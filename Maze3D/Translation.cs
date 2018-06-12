using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using System.Globalization;
using System;

public sealed class Translation : GameComponent
{
    private static Dictionary<string, string> Translations = new Dictionary<string, string>();

    public Translation(Game game) : base(game)
    {
        if (Translations.Count > 0)
            return;

        var userLanguage = CultureInfo.CurrentCulture.Name.Split('-')[0];

        var path = $"Content/Data/Translations/translation.{userLanguage}.ini";
        if (!File.Exists(path))
            path = "Content/Data/Translations/translation.en.ini";

        var data = File.ReadAllText(path);
        if (data == null)
            throw new Exception($"The translation file {path} is missing.");

        ParseFile(data);
    }

    // Returns the translation for this key.
    public static string Get(string key, int recursion = 0)
    {
        if (Translations.ContainsKey(key))
        {
            var str = Translations[key];
            if (str.StartsWith("#"))
            {
                if (recursion > 5)
                    return key;

                var sub = str.Replace("#", "");
                return Get(sub, recursion + 1);
            }

            return Translations[key];
        }
        return key;
    }

    public void ParseFile(string data)
    {
        using (var stream = new StringReader(data))
        {
            var line = stream.ReadLine();
            var temp = new string[2];
            var key = string.Empty;
            var value = string.Empty;

            while (line != null)
            {
                if (line.StartsWith(";"))
                {
                    line = stream.ReadLine();
                    continue;
                }

                temp = line.Split('=');

                if (temp.Length == 2)
                {
                    key = temp[0].Trim();
                    value = temp[1].Trim();

                    if (value != string.Empty)
                    {
                        if (Translations.ContainsKey(key))
                            Translations[key] = value;
                        else
                            Translations.Add(key, value);
                    }
                }

                line = stream.ReadLine();
            }
        }
    }
}

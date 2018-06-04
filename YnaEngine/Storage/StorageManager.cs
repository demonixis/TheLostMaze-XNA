// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Yna.Engine.Storage
{
    /// <summary>
    /// The storage manager is an object that can be used for store and load informations like scores, achievments, etc..
    /// </summary>
    public class StorageManager
    {
        private string _saveFolder;

        public StorageManager()
        {
            _saveFolder = GetSaveContainer();
        }

        private string GetSaveContainer()
        {
            var builder = new StringBuilder();
            builder.Append(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            builder.Append(Path.DirectorySeparatorChar);
            builder.Append("my games");
            builder.Append(Path.DirectorySeparatorChar);
            builder.Append(YnGame.GameTitle);
            builder.Append(Path.DirectorySeparatorChar);
            return builder.ToString();
        }

        private string GetContainer(string containerName)
        {
            string containerTarget = _saveFolder + containerName;

            if (!Directory.Exists(containerTarget))
                Directory.CreateDirectory(containerTarget);

            return containerTarget;
        }

        /// <summary>
        /// Save a serializable object in the user's local storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the user's storage. If the folder doesn't exist, it's created</param>
        /// <param name="fileName">The file's name</param>
        /// <param name="obj">Serializable object</param>
        public virtual void Save<T>(string containerName, string fileName, T obj)
        {
            string container = GetContainer(containerName);
            string filePath = GetFilePath(container, containerName, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath); // TODO : backup file

            StreamWriter writer = new StreamWriter(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            serializer.Serialize(writer, obj);
            writer.Dispose();
        }

        /// <summary>
        /// Load a serialized object from the user's local storage
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="containerName">Folder in the user's storage.</param>
        /// <param name="fileName">The file's name</param>
        /// <returns>Instance of the object type with previous saved datas</returns>
        public virtual T Load<T>(string containerName, string fileName)
        {
            T datas = default(T);

            string container = GetContainer(containerName);
            string filePath = GetFilePath(container, containerName, fileName);

            if (File.Exists(filePath))
            {
                Stream stream = File.Open(filePath, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                datas = (T)serializer.Deserialize(stream);
                stream.Dispose();
            }

            return datas;
        }

        private string GetFilePath(string container, string containerName, string fileName)
        {
            StringBuilder pathBuilder = new StringBuilder();
            pathBuilder.Append(container);

            if (containerName != String.Empty)
                pathBuilder.Append(Path.DirectorySeparatorChar);

            pathBuilder.Append(fileName);

            return pathBuilder.ToString();
        }
    }
}

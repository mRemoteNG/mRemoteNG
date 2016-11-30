using System;
using System.Collections.Generic;
using System.IO;

namespace mRemoteNG.App.Update
{
    public class UpdateFile
    {
        #region Public Properties
        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable once MemberCanBePrivate.Global
        public Dictionary<string, string> Items { get; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        #endregion

        #region Public Methods
        public UpdateFile(string content)
        {
            FromString(content);
        }

        // ReSharper disable MemberCanBePrivate.Local
        // ReSharper disable once MemberCanBePrivate.Global
        public void FromString(string content)
        {
            if (string.IsNullOrEmpty(content)) return;

            char[] keyValueSeparators = { ':', '=' };
            char[] commentCharacters = { '#', ';', '\'' };

            using (var sr = new StringReader(content))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var trimmedLine = line.Trim();
                    if (trimmedLine.Length == 0)
                    {
                        continue;
                    }
                    if (trimmedLine.Substring(0, 1).IndexOfAny(commentCharacters) != -1)
                    {
                        continue;
                    }

                    var parts = trimmedLine.Split(keyValueSeparators, 2);
                    if (parts.Length != 2)
                    {
                        continue;
                    }
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    Items.Add(key, value);
                }
            }
        }

        // ReSharper disable MemberCanBePrivate.Local
        private string GetString(string key)
        {
            // ReSharper restore MemberCanBePrivate.Local
            return !Items.ContainsKey(key) ? string.Empty : this.Items[key];
        }

        public Version GetVersion(string key)
        {
            var value = GetString(key);
            return string.IsNullOrEmpty(value) ? null : new Version(value);
        }

        public Uri GetUri(string key)
        {
            var value = GetString(key);
            return string.IsNullOrEmpty(value) ? null : new Uri(value);
        }

        public string GetThumbprint(string key)
        {
            return GetString(key).Replace(" ", "").ToUpperInvariant();
        }

        public string GetFileName()
        {
            var value = GetString("dURL");
            var sv = value.Split('/');
            return sv[sv.Length-1];
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;

namespace mRemoteNG.App.Update
{
    public class UpdateFile
    {
        #region Public Properties
        private Dictionary<string, string> _items = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        // ReSharper disable MemberCanBePrivate.Local
        public Dictionary<string, string> Items
        {
            // ReSharper restore MemberCanBePrivate.Local
            get
            {
                return _items;
            }
        }
        #endregion

        #region Public Methods
        public UpdateFile(string content)
        {
            FromString(content);
        }

        // ReSharper disable MemberCanBePrivate.Local
        public void FromString(string content)
        {
            // ReSharper restore MemberCanBePrivate.Local
            if (string.IsNullOrEmpty(content))
            {
            }
            else
            {
                char[] lineSeparators = new char[] { '\n', '\r' };
                char[] keyValueSeparators = new char[] { ':', '=' };
                char[] commentCharacters = new char[] { '#', ';', '\'' };

                string[] lines = content.Split(lineSeparators.ToString().ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (trimmedLine.Length == 0)
                    {
                        continue;
                    }
                    if (!(trimmedLine.Substring(0, 1).IndexOfAny(commentCharacters.ToString().ToCharArray()) == -1))
                    {
                        continue;
                    }

                    string[] parts = trimmedLine.Split(keyValueSeparators.ToString().ToCharArray(), 2);
                    if (!(parts.Length == 2))
                    {
                        continue;
                    }
                    string key = System.Convert.ToString(parts[0].Trim());
                    string value = System.Convert.ToString(parts[1].Trim());

                    _items.Add(key, value);
                }
            }
        }

        // ReSharper disable MemberCanBePrivate.Local
        public string GetString(string key)
        {
            // ReSharper restore MemberCanBePrivate.Local
            if (!Items.ContainsKey(key))
            {
                return string.Empty;
            }
            return this._items[key];
        }

        public Version GetVersion(string key)
        {
            string value = GetString(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new Version(value);
        }

        public Uri GetUri(string key)
        {
            string value = GetString(key);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            return new Uri(value);
        }

        public string GetThumbprint(string key)
        {
            return GetString(key).Replace(" ", "").ToUpperInvariant();
        }
        #endregion
    }
}
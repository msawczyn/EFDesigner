using Microsoft.Msagl.Core.Geometry.Curves;

namespace Microsoft.Msagl.Prototype.LayoutEditing {
    /// <summary>
    /// node restore data
    /// </summary>
    public class NodeRestoreData:RestoreData {
        
        internal NodeRestoreData(ICurve boundaryCurve) {
            this.boundaryCurve = boundaryCurve;
        }

        private ICurve boundaryCurve;

        /// <summary>
        /// node boundary curve
        /// </summary>
        public ICurve BoundaryCurve {
            get { return boundaryCurve; }
            set { boundaryCurve = value; }
        }
    }
}

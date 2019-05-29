using System.Collections.Generic;

namespace Microsoft.Msagl.Routing.Spline.Bundling {
    /// <summary>
    /// 
    /// </summary>
    interface IMetroMapOrderingAlgorithm {
        IEnumerable<Metroline> GetOrder(Station u, Station v);

        /// <summary>
        /// Get the index of line on the edge (u->v) and node u
        /// </summary>
        int GetLineIndexInOrder(Station u, Station v, Metroline metroLine);
    }



}

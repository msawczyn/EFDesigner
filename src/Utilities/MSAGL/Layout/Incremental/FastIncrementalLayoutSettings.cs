using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Msagl.Core;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Layout;

namespace Microsoft.Msagl.Layout.Incremental
{
   /// <summary>
   ///    Fast incremental layout settings
   /// </summary>
#if PROPERTY_GRID_SUPPORT
   [DisplayName("Fast Incremental layout settings")]
   [Description("Settings for Fast Incremental Layout algorithm")]
   [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
   public class FastIncrementalLayoutSettings : LayoutAlgorithmSettings
   {
      internal FastIncrementalLayout algorithm;

      private double decay = 0.9;

      private double friction = 0.8;

      private double initialStepSize = 1.4;

      internal LinkedList<LockPosition> locks = new LinkedList<LockPosition>();

      private int maxConstraintLevel = 2;
      internal List<IConstraint> structuralConstraints = new List<IConstraint>();

      /// <summary>
      /// </summary>
      public FastIncrementalLayoutSettings() { }

      /// <summary>
      ///    Shallow copy the settings
      /// </summary>
      /// <param name="previousSettings"></param>
      public FastIncrementalLayoutSettings(FastIncrementalLayoutSettings previousSettings)
      {
         ValidateArg.IsNotNull(previousSettings, "previousSettings");
         MaxIterations = previousSettings.MaxIterations;
         MinorIterations = previousSettings.MinorIterations;
         ProjectionIterations = previousSettings.ProjectionIterations;
         ApproximateRepulsion = previousSettings.ApproximateRepulsion;
         initialStepSize = previousSettings.initialStepSize;
         RungeKuttaIntegration = previousSettings.RungeKuttaIntegration;
         decay = previousSettings.decay;
         friction = previousSettings.friction;
         RepulsiveForceConstant = previousSettings.RepulsiveForceConstant;
         AttractiveForceConstant = previousSettings.AttractiveForceConstant;
         GravityConstant = previousSettings.GravityConstant;
         InterComponentForces = previousSettings.InterComponentForces;
         ApplyForces = previousSettings.ApplyForces;
         IdealEdgeLength = previousSettings.IdealEdgeLength;
         AvoidOverlaps = previousSettings.AvoidOverlaps;
         RespectEdgePorts = previousSettings.RespectEdgePorts;
         RouteEdges = previousSettings.RouteEdges;
         ApproximateRouting = previousSettings.ApproximateRouting;
         LogScaleEdgeForces = previousSettings.LogScaleEdgeForces;
         DisplacementThreshold = previousSettings.DisplacementThreshold;
         MinConstraintLevel = previousSettings.MinConstraintLevel;
         maxConstraintLevel = previousSettings.maxConstraintLevel;
         AttractiveInterClusterForceConstant = previousSettings.AttractiveInterClusterForceConstant;
         ClusterGravity = previousSettings.ClusterGravity;
         PackingAspectRatio = previousSettings.PackingAspectRatio;
         NodeSeparation = previousSettings.NodeSeparation;
         ClusterMargin = previousSettings.ClusterMargin;
      }

      /// <summary>
      ///    Stop after maxIterations completed
      /// </summary>
      public int MaxIterations { get; set; } = 100;

      /// <summary>
      ///    Number of iterations in inner loop.
      /// </summary>
      public int MinorIterations { get; set; } = 3;

      /// <summary>
      ///    Number of iterations completed
      /// </summary>
      [Browsable(false)]
      public int Iterations { get; set; }

      /// <summary>
      ///    number of times to project over all constraints at each layout iteration
      /// </summary>
      public int ProjectionIterations { get; set; } = 5;

      /// <summary>
      ///    Rather than computing the exact repulsive force between all pairs of nodes (which would take O(n^2) time for n
      ///    nodes)
      ///    use a fast inexact technique (that takes O(n log n) time)
      /// </summary>
      public bool ApproximateRepulsion { get; set; } = true;

      /// <summary>
      ///    RungaKutta integration potentially gives smoother increments, but is more expensive
      /// </summary>
      public bool RungeKuttaIntegration { get; set; }

      /// <summary>
      ///    StepSize taken at each iteration (a coefficient of the force on each node) adapts depending on change in
      ///    potential energy at each step.  With this scheme changing the InitialStepSize doesn't have much effect
      ///    because if it is too large or too small it will be quickly updated by the algorithm anyway.
      /// </summary>
      public double InitialStepSize
      {
         get { return initialStepSize; }
         set
         {
            // ForceScalar should be greater than 0 and less than 2 (if we let you set it to 0 nothing would happen,
            // greater than 2 would most likely be very unstable!)
            initialStepSize = Math.Min(2, Math.Max(0.001, value));
         }
      }

      /// <summary>
      ///    FrictionalDecay isn't really friction so much as a scaling of velocity to improve convergence.  0.8 seems to work
      ///    well.
      /// </summary>
      public double Decay
      {
         get { return decay; }
         set
         {
            // Setting decay too small gives no progress.  1==no decay, 0.1==minimum allowed value
            decay = Math.Min(1, Math.Max(0.1, value));
         }
      }

      /// <summary>
      ///    Friction isn't really friction so much as a scaling of velocity to improve convergence.  0.8 seems to work well.
      /// </summary>
      public double Friction
      {
         get { return friction; }
         set
         {
            // Setting friction less than 0 or greater than 1 would just be strange.  1==no friction, 0==no conservation of velocity
            friction = Math.Max(1, Math.Min(0, value));
         }
      }

      /// <summary>
      ///    strength of repulsive force between each pair of nodes.  A setting of 1.0 should work OK.
      /// </summary>
      public double RepulsiveForceConstant { get; set; } = 1.0;

      /// <summary>
      ///    strength of attractive force between pairs of nodes joined by an edge.  A setting of 1.0 should work OK.
      /// </summary>
      public double AttractiveForceConstant { get; set; } = 1.0;

      /// <summary>
      ///    gravity is a constant force applied to all nodes attracting them to the Origin
      ///    and keeping disconnected components from flying apart.  A setting of 1.0 should work OK.
      /// </summary>
      public double GravityConstant { get; set; } = 1.0;

      /// <summary>
      ///    If the following is false forces will not be considered between each component and each component will have its own
      ///    gravity origin.
      /// </summary>
      public bool InterComponentForces { get; set; } = true;

      /// <summary>
      ///    If the following is false forces will not be applied, but constraints will still be satisfied.
      /// </summary>
      public bool ApplyForces { get; set; } = true;

      /// <summary>
      /// </summary>
      [Browsable(false)]
      public bool IsInitialized
      {
         get { return algorithm != null; }
      }

      /// <summary>
      /// </summary>
      [Browsable(false)]
      public IEnumerable<IConstraint> StructuralConstraints
      {
         get { return structuralConstraints; }
      }

      /// <summary>
      ///    Avoid overlaps between nodes boundaries, and if there are any
      ///    clusters, then between each cluster boundary and nodes that are not
      ///    part of that cluster.
      /// </summary>
      public bool AvoidOverlaps { get; set; }

      /// <summary>
      ///    If edges have FloatingPorts then the layout will optimize edge lengths based on the port locations.
      ///    If MultiLocationFloatingPorts are specified then the layout will choose the nearest pair of locations for each such
      ///    edge.
      /// </summary>
      public bool RespectEdgePorts { get; set; }

      /// <summary>
      ///    Apply nice but expensive routing of edges once layout converges
      /// </summary>
      public bool RouteEdges { get; set; }

      /// <summary>
      ///    If RouteEdges is true then the following is checked to see whether to do optimal shortest path routing
      ///    or use a sparse visibility graph spanner to do approximate---but much faster---shortest path routing
      /// </summary>
      public bool ApproximateRouting { get; set; } = true;

      /// <summary>
      ///    If true then attractive forces across edges are computed as:
      ///    AttractiveForceConstant * actualLength * Math.Log((actualLength + epsilon) / (idealLength + epsilon))
      ///    where epsilon is a small positive constant to avoid divide by zero or taking the log of zero.
      ///    Note that LogScaleEdges can lead to ghost forces in highly constrained scenarios.
      ///    If false then a the edge force is based on (actualLength - idealLength)^2, which works better with
      ///    lots of constraints.
      /// </summary>
      public bool LogScaleEdgeForces { get; set; } = true;

      /// <summary>
      ///    If the amount of total squared displacement after a particular iteration falls below DisplacementThreshold then
      ///    Converged is set to true.
      ///    Make DisplacementThreshold larger if you want layout to finish sooner - but not necessarily make as much progress
      ///    towards a good layout.
      /// </summary>
      [Browsable(false)]
      public double DisplacementThreshold { get; set; } = 0.1;

      /// <summary>
      ///    Set to true if displacement from the last iteration was less than DisplacementThreshold.
      ///    The caller should invoke FastIncrementalLayout.CalculateLayout() in a loop, e.g.:
      ///    while(!settings.Converged)
      ///    {
      ///    layout.CalculateLayout();
      ///    redrawGraphOrHandleInteractionOrWhatever();
      ///    }
      ///    RemainingIterations affects damping.
      /// </summary>
      [Browsable(false)]
      public bool Converged { get; set; }

      /// <summary>
      ///    Return iterations as a percentage of MaxIterations.  Useful for reporting progress, e.g. in a progress bar.
      /// </summary>
      [Browsable(false)]
      public int PercentDone
      {
         get
         {
            return Converged
                      ? 100
                      : (int)(100.0 * Iterations / MaxIterations);
         }
      }

      /// <summary>
      ///    Not quite the same as Converged:
      /// </summary>
      [Browsable(false)]
      public bool IsDone
      {
         get { return Converged || Iterations >= MaxIterations; }
      }

      /// <summary>
      ///    Returns an estimate of the cost function calculated in the most recent iteration.
      ///    It's a float because FastIncrementalLayout.Energy is a volatile float so it
      ///    can be safely read from other threads
      /// </summary>
      [Browsable(false)]
      public float Energy
      {
         get
         {
            return algorithm?.energy ?? 0;
         }
      }

      /// <summary>
      ///    When layout is in progress the following is false.
      ///    When layout has converged, routes are populated and this is set to true to tell the UI that the routes can be drawn.
      /// </summary>
      [Browsable(false)]
      public bool EdgeRoutesUpToDate { get; set; }

      /// <summary>
      /// </summary>
      [Browsable(false)]
      public int MaxConstraintLevel
      {
         get
         {
            return maxConstraintLevel;
         }
         set
         {
            if (maxConstraintLevel != value)
            {
               maxConstraintLevel = value;

               if (IsInitialized)
                  Uninitialize();
            }
         }
      }

      /// <summary>
      /// </summary>
      [Browsable(false)]
      public int MinConstraintLevel { get; set; }

      /// <summary>
      ///    Constraint level ranges from Min to MaxConstraintLevel.
      ///    0 = no constraints
      ///    1 = only structural constraints
      ///    2 = all constraints including non-overlap constraints
      ///    A typical run of FastIncrementalLayout will apply it at each constraint level, starting at 0 to
      ///    obtain an untangled unconstrained layout, then 1 to introduce structural constraints and finally 2 to beautify.
      ///    Running only at level 2 will most likely leave the graph stuck in a tangled local minimum.
      /// </summary>
      [Browsable(false)]
      public int CurrentConstraintLevel
      {
         get { return algorithm?.CurrentConstraintLevel ?? 0; }
         set
         {
            if (algorithm != null)
               algorithm.CurrentConstraintLevel = value;
         }
      }

      /// <summary>
      ///    Attractive strength of edges connected to clusters
      /// </summary>
      public double AttractiveInterClusterForceConstant { get; set; } = 1.0;

      /// <summary>
      ///    Controls how tightly members of clusters are pulled together
      /// </summary>
      public double ClusterGravity { get; set; } = 1.0;

      /// <summary>
      ///    Settings for calculation of ideal edge length
      /// </summary>
      public IdealEdgeLengthSettings IdealEdgeLength { get; set; }

      /// <summary>
      ///    Force groups to follow their constituent nodes,
      ///    true by default.
      /// </summary>
      public bool UpdateClusterBoundariesFromChildren { get; set; } = true;

      /// <summary>
      /// </summary>
      public void AddStructuralConstraint(IConstraint cc)
      {
         structuralConstraints.Add(cc);
      }

      /// <summary>
      ///    Clear all constraints over the graph
      /// </summary>
      public void ClearConstraints()
      {
         locks.Clear();
         structuralConstraints.Clear();
      }

      /// <summary>
      ///    Remove all locks on node positions
      /// </summary>
      public void ClearLocks()
      {
         locks.Clear();
      }

      /// <summary>
      /// </summary>
      public void ClearStructuralConstraints()
      {
         structuralConstraints.Clear();
      }

      /// <summary>
      ///    Clones the object
      /// </summary>
      /// <returns></returns>
      public override LayoutAlgorithmSettings Clone()
      {
         return MemberwiseClone() as LayoutAlgorithmSettings;
      }

      /// <summary>
      ///    creates the settings that seems working
      /// </summary>
      /// <returns></returns>
      public static FastIncrementalLayoutSettings CreateFastIncrementalLayoutSettings()
      {
         return new FastIncrementalLayoutSettings
         {
            ApplyForces                         = false,
            ApproximateRepulsion                = true,
            ApproximateRouting                  = true,
            AttractiveForceConstant             = 1.0,
            AttractiveInterClusterForceConstant = 1.0,
            AvoidOverlaps                       = true,
            ClusterGravity                      = 1.0,
            Decay                               = 0.9,
            DisplacementThreshold               = 0.00000005,
            Friction                            = 0.8,
            GravityConstant                     = 1.0,
            InitialStepSize                     = 2.0,
            InterComponentForces                = false,
            Iterations                          = 0,
            LogScaleEdgeForces                  = false,
            MaxConstraintLevel                  = 2,
            MaxIterations                       = 20,
            MinConstraintLevel                  = 0,
            MinorIterations                     = 1,
            ProjectionIterations                = 5,
            RepulsiveForceConstant              = 2.0,
            RespectEdgePorts                    = false,
            RouteEdges                          = false,
            RungeKuttaIntegration               = true,
            UpdateClusterBoundariesFromChildren = true,
            NodeSeparation                      = 20
         };
      }

      /// <summary>
      ///    Add a LockPosition for each node whose position you want to keep fixed.  LockPosition allows you to,
      ///    for example, do interactive mouse
      ///    dragging.
      ///    We return the LinkedListNode which you can store together with your local Node object so that a RemoveLock operation
      ///    can be performed in
      ///    constant time.
      /// </summary>
      /// <param name="node"></param>
      /// <param name="bounds"></param>
      /// <returns>LinkedListNode which you should hang on to if you want to call RemoveLock later on.</returns>
      public LockPosition CreateLock(Node node, Rectangle bounds)
      {
         LockPosition lp = new LockPosition(node, bounds);
         lp.listNode = locks.AddLast(lp);

         return lp;
      }

      /// <summary>
      ///    Add a LockPosition for each node whose position you want to keep fixed.  LockPosition allows you to,
      ///    for example, do interactive mouse dragging.
      ///    We return the LinkedListNode which you can store together with your local Node object so that a RemoveLock operation
      ///    can be performed in
      ///    constant time.
      /// </summary>
      /// <param name="node"></param>
      /// <param name="bounds"></param>
      /// <param name="weight">stay weight of lock</param>
      /// <returns>LinkedListNode which you should hang on to if you want to call RemoveLock later on.</returns>
      public LockPosition CreateLock(Node node, Rectangle bounds, double weight)
      {
         LockPosition lp = new LockPosition(node, bounds, weight);
         lp.listNode = locks.AddLast(lp);

         return lp;
      }

      /// <summary>
      /// </summary>
      public void IncrementalRun(GeometryGraph graph)
      {
         IncrementalRun(graph, anyCluster => this);
      }

      /// <summary>
      ///    Run the FastIncrementalLayout instance incrementally
      /// </summary>
      public void IncrementalRun(GeometryGraph graph, Func<Cluster, LayoutAlgorithmSettings> clusterSettings)
      {
         SetupIncrementalRun(graph, clusterSettings);
         algorithm?.Run();
         graph.UpdateBoundingBox();
      }

      /// <summary>
      /// </summary>
      public void IncrementalRun(CancelToken cancelToken, GeometryGraph graph, Func<Cluster, LayoutAlgorithmSettings> clusterSettings)
      {
         cancelToken?.ThrowIfCanceled();
         SetupIncrementalRun(graph, clusterSettings);
         algorithm?.Run(cancelToken);
         graph.UpdateBoundingBox();
      }

      /// <summary>
      /// </summary>
      public void InitializeLayout(GeometryGraph graph, int initialConstraintLevel)
      {
         InitializeLayout(graph, initialConstraintLevel, anyCluster => this);
      }

      /// <summary>
      ///    Initialize the layout algorithm
      /// </summary>
      /// <param name="graph">The graph upon which layout is performed</param>
      /// <param name="initialConstraintLevel"></param>
      /// <param name="clusterSettings"></param>
      public void InitializeLayout(GeometryGraph graph, int initialConstraintLevel, Func<Cluster, LayoutAlgorithmSettings> clusterSettings)
      {
         ValidateArg.IsNotNull(graph, "graph");
         algorithm = new FastIncrementalLayout(graph, this, initialConstraintLevel, clusterSettings);
         ResetLayout();
      }

      /// <summary>
      ///    Remove a specific lock on node position.  Once you remove it, you'll have to call AddLock again to create a new one
      ///    if you want to lock it again.
      /// </summary>
      /// <param name="lockPosition">the LinkedListNode returned by the AddLock method above</param>
      [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "FastIncrementalLayoutSettings")]
      [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "RemoveLock")]
      public void RemoveLock(LockPosition lockPosition)
      {
         ValidateArg.IsNotNull(lockPosition, "lockPosition");

         if (lockPosition.listNode != null)
         {
            lockPosition.RestoreNodeWeight();

            try
            {
               locks.Remove(lockPosition.listNode);
            }
            catch (InvalidOperationException e)
            {
               Console.WriteLine("Problem in FastIncrementalLayoutSettings.RemoveLock " + e.Message);
            }

            lockPosition.listNode = null;
         }
      }

      /// <summary>
      ///    restart layout, use e.g. after a mouse drag or non-structural change to the graph
      /// </summary>
      public void ResetLayout()
      {
         Unconverge();

         if (algorithm != null)
         {
            algorithm.ResetNodePositions();
            algorithm.SetLockNodeWeights();
         }
      }

      private void SetupIncrementalRun(GeometryGraph graph, Func<Cluster, LayoutAlgorithmSettings> clusterSettings)
      {
         ValidateArg.IsNotNull(graph, "graph");

         if (!IsInitialized)
         {
            InitializeLayout(graph, MaxConstraintLevel, clusterSettings);
         }
         else if (IsDone)
         {
            // If we were already done from last time but we are doing more work then something has changed.
            ResetLayout();
         }
      }

      /// <summary>
      ///    reset iterations and convergence status
      /// </summary>
      internal void Unconverge()
      {
         Iterations = 0;

         //EdgeRoutesUpToDate = false;
         Converged = false;
      }

      /// <summary>
      /// </summary>
      public void Uninitialize()
      {
         algorithm = null;
      }

#if PROPERTY_GRID_SUPPORT
      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return "Fast Incremental Layout Settings";
      }
#endif

   }
}
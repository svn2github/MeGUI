using System;
using System.Collections.Generic;
using System.Text;

namespace MeGUI
{
    /// <summary>
    /// Provides a thin wrapper for a list of MuxPathLegs and adds some extra MuxPath-specific functionality
    /// </summary>
    public class MuxPath
    {
        private List<MuxPathLeg> path;
        private List<MuxableType> initialInputTypes;
        private ContainerType targetType;

        public ContainerType TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        public List<MuxableType> InitialInputTypes
        {
            get { return initialInputTypes; }
            set { initialInputTypes = value; }
        }

        public int Length
        {
            get { return path.Count; }
        }

        public void Add(MuxPathLeg leg)
        {
            path.Add(leg);
        }

        public MuxPath(ContainerType targetType)
        {
            path = new List<MuxPathLeg>();
            initialInputTypes = new List<MuxableType>();
            this.targetType = targetType;
        }

        public MuxPath(IEnumerable<MuxableType> initialInputTypes, ContainerType targetType)
            : this(targetType)
        {
            this.initialInputTypes.AddRange(initialInputTypes);
        }

        public MuxPath Clone()
        {
            MuxPath nMuxPath = new MuxPath(initialInputTypes, targetType);
            nMuxPath.path.AddRange(path);
            return nMuxPath;
        }

        public IEnumerator<MuxPathLeg> GetEnumerator()
        {
            return path.GetEnumerator();
        }

        public MuxPathLeg this[int index]
        {
            get { return path[index]; }
        }

        public bool IsCompleted()
        {
            if (path.Count == 0)
            {
                return (initialInputTypes.Count == 0 || 
                    (initialInputTypes.Count == 1 &&
                    (initialInputTypes[0].outputType.ContainerType == this.targetType)) );
            }
            else
            {
                return (path[path.Count - 1].unhandledInputTypes.Count == 0 &&
                    path[path.Count - 1].muxerInterface.GetSupportedContainerTypes().Contains(targetType));
            }
        }
    }
}

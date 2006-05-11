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
        private List<OutputType> initialInputTypes;
        private int initialInputFiles = 0;
        private ContainerType targetType;

        public ContainerType TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        public List<OutputType> InitialInputTypes
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
            initialInputTypes = new List<OutputType>();
            this.targetType = targetType;
        }

        public MuxPath(IEnumerable<OutputType> initialInputTypes, int initialInputFiles, ContainerType targetType)
            : this(targetType)
        {
            this.initialInputTypes.AddRange(initialInputTypes);
            this.initialInputFiles = initialInputFiles;
        }

        public MuxPath Clone()
        {
            MuxPath nMuxPath = new MuxPath(initialInputTypes, initialInputFiles, targetType);
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
                    (initialInputTypes.Count == 1 && initialInputFiles == 1 &&
                    (initialInputTypes[0].ContainerType == this.targetType)) );
            }
            else
            {
                return (path[path.Count - 1].unhandledInputTypes.Count == 0 &&
                    path[path.Count - 1].muxerInterface.GetSupportedContainerTypes().Contains(targetType));
            }
        }
    }
}

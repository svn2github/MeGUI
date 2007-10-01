using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using MeGUI.core.util;

namespace MeGUI
{
    public delegate void MuxerOutputCallback(string line, int type);

    abstract class CommandlineMuxer : CommandlineJobProcessor<MuxJob>
    {
        void setProjectedFileSize()
        {
            su.ProjectedFileSize = FileSize.Empty;
            su.ProjectedFileSize += (FileSize.Of2(job.Settings.VideoInput) ?? FileSize.Empty);
            su.ProjectedFileSize += (FileSize.Of2(job.Settings.MuxedInput) ?? FileSize.Empty);

            foreach (MuxStream s in job.Settings.AudioStreams)
                su.ProjectedFileSize += FileSize.Of2(s.path) ?? FileSize.Empty;

            foreach (MuxStream s in job.Settings.SubtitleStreams)
                su.ProjectedFileSize += FileSize.Of2(s.path) ?? FileSize.Empty;
        }

        protected override void checkJobIO()
        {
            ensureInputFilesExistIfNeeded(job.Settings);
            setProjectedFileSize();
        }

        private void ensureInputFilesExistIfNeeded(MuxSettings settings)
        {
            Util.ensureExistsIfNeeded(settings.MuxedInput);
            Util.ensureExistsIfNeeded(settings.VideoInput);
            Util.ensureExistsIfNeeded(settings.ChapterFile);
            foreach (MuxStream s in settings.AudioStreams)
                Util.ensureExistsIfNeeded(s.path);
            foreach (MuxStream s in settings.SubtitleStreams)
                Util.ensureExistsIfNeeded(s.path);
        }
    }
}

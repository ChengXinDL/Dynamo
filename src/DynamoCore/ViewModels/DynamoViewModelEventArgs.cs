﻿using System;
using System.Collections.Generic;
using System.Windows;

using Dynamo.Models;

namespace Dynamo.ViewModels
{
    public class ZoomEventArgs : EventArgs
    {
        internal enum ZoomModes
        {
            ByPoint = 0x00000001,
            ByFactor = 0x00000002,
            ByFitView = 0x00000004
        }

        internal Point Point { get; set; }
        internal double Zoom { get; set; }
        internal ZoomModes Modes { get; private set; }

        internal Point Offset { get; set; }
        internal double FocusWidth { get; set; }
        internal double FocusHeight { get; set; }

        internal ZoomEventArgs(double zoom)
        {
            Zoom = zoom;
            this.Modes = ZoomModes.ByFactor;
        }

        internal ZoomEventArgs(Point point)
        {
            this.Point = point;
            this.Modes = ZoomModes.ByPoint;
        }

        internal ZoomEventArgs(double zoom, Point point)
        {
            this.Point = point;
            this.Zoom = zoom;
            this.Modes = ZoomModes.ByPoint | ZoomModes.ByFactor;
        }

        internal ZoomEventArgs(Point offset, double focusWidth, double focusHeight)
        {
            this.Offset = offset;
            this.FocusWidth = focusWidth;
            this.FocusHeight = focusHeight;
            this.Modes = ZoomModes.ByFitView;
        }

        internal ZoomEventArgs(Point offset, double focusWidth, double focusHeight, double zoom)
        {
            this.Offset = offset;
            this.FocusWidth = focusWidth;
            this.FocusHeight = focusHeight;
            this.Zoom = zoom;
            this.Modes = ZoomModes.ByFitView | ZoomModes.ByFactor;
        }

        internal bool hasPoint()
        {
            return this.Modes.HasFlag(ZoomModes.ByPoint);
        }

        internal bool hasZoom()
        {
            return this.Modes.HasFlag(ZoomModes.ByFactor);
        }
    }

    public class NoteEventArgs : EventArgs
    {
        public NoteModel Note { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public NoteEventArgs(NoteModel n, Dictionary<string, object> d)
        {
            Note = n;
            Data = d;
        }
    }

    public class ViewEventArgs : EventArgs
    {
        public object View { get; set; }

        public ViewEventArgs(object v)
        {
            View = v;
        }
    }

    public class SelectionBoxUpdateArgs : EventArgs
    {
        public enum UpdateFlags
        {
            Position = 0x00000001,
            Dimension = 0x00000002,
            Visibility = 0x00000004,
            Mode = 0x00000008
        }

        public SelectionBoxUpdateArgs(Visibility visibility)
        {
            this.Visibility = visibility;
            this.UpdatedProps = UpdateFlags.Visibility;
        }

        public SelectionBoxUpdateArgs(double x, double y)
        {
            this.X = x;
            this.Y = y;
            this.UpdatedProps = UpdateFlags.Position;
        }

        public SelectionBoxUpdateArgs(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.UpdatedProps = UpdateFlags.Position | UpdateFlags.Dimension;
        }

        public void SetSelectionMode(bool isCrossSelection)
        {
            this.IsCrossSelection = isCrossSelection;
            this.UpdatedProps |= UpdateFlags.Mode;
        }

        public void SetVisibility(Visibility visibility)
        {
            this.Visibility = visibility;
            this.UpdatedProps |= UpdateFlags.Visibility;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public bool IsCrossSelection { get; private set; }
        public Visibility Visibility { get; private set; }
        public UpdateFlags UpdatedProps { get; private set; }
    }

    public class WorkspaceSaveEventArgs : EventArgs
    {
        public WorkspaceModel Workspace { get; set; }
        public bool AllowCancel { get; set; }
        public bool Success { get; set; }
        public WorkspaceSaveEventArgs(WorkspaceModel ws, bool allowCancel)
        {
            Workspace = ws;
            AllowCancel = allowCancel;
            Success = false;
        }
    }

    public class ImageSaveEventArgs : EventArgs
    {
        public string Path { get; set; }

        public ImageSaveEventArgs(string path)
        {
            Path = path;
        }
    }

}

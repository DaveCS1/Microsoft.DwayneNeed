﻿using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Microsoft.DwayneNeed.MDI
{
    public class MdiWindowThumb : Thumb
    {
        /// <summary>
        ///     The edge that the thumb should interact with.
        /// </summary>
        public static DependencyProperty InteractiveEdgesProperty = DependencyProperty.Register(
            /* Name:                 */ "InteractiveEdges",
            /* Value Type:           */ typeof(MdiWindowEdge),
            /* Owner Type:           */ typeof(MdiWindowThumb),
            /* Metadata:             */ new FrameworkPropertyMetadata(
                /*     Default Value:    */ MdiWindowEdge.None,
                /*     Change Callback:  */ (s, e) => ((MdiWindowThumb) s).OnInteractiveEdgesChanged(e)));

        /// <summary>
        ///     The command to raise when the thumb is double clicked.
        /// </summary>
        public static DependencyProperty DoubleClickCommandProperty = DependencyProperty.Register(
            /* Name:                 */ "DoubleClickCommand",
            /* Value Type:           */ typeof(RoutedCommand),
            /* Owner Type:           */ typeof(MdiWindowThumb),
            /* Metadata:             */ new FrameworkPropertyMetadata(
                /*     Default Value:    */ null));

        static MdiWindowThumb()
        {
            // Look up the style for this control by using its type as its key.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MdiWindowThumb),
                new FrameworkPropertyMetadata(typeof(MdiWindowThumb)));

            EventManager.RegisterClassHandler(
                typeof(MdiWindowThumb),
                DragDeltaEvent,
                (DragDeltaEventHandler) ((s, e) => ((MdiWindowThumb) s).OnDragDelta(e)));

            CursorProperty.OverrideMetadata(
                /* Type:                 */ typeof(MdiWindowThumb),
                /* Metadata:             */ new FrameworkPropertyMetadata(
                    /*     Default Value:    */ Cursors.Arrow,
                    /*     Changed Callback: */ delegate { },
                    /*     Coerce Callback:  */ (d, v) => ((MdiWindowThumb) d).OnCoerceCursor(v)));
        }

        /// <summary>
        ///     The edge that the thumb should interact with.
        /// </summary>
        public MdiWindowEdge InteractiveEdges
        {
            get => (MdiWindowEdge) GetValue(InteractiveEdgesProperty);
            set => SetValue(InteractiveEdgesProperty, value);
        }

        /// <summary>
        ///     The command to raise when the thumb is double clicked.
        /// </summary>
        public RoutedCommand DoubleClickCommand
        {
            get => (RoutedCommand) GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (DoubleClickCommand != null) DoubleClickCommand.Execute(null, this);

            base.OnMouseDoubleClick(e);
        }

        private void OnDragDelta(DragDeltaEventArgs e)
        {
            AdjustWindowRectParameter swp = new AdjustWindowRectParameter
            {
                InteractiveEdges = InteractiveEdges,
                Delta = new Vector(e.HorizontalChange, e.VerticalChange)
            };

            MdiCommands.AdjustWindowRect.Execute(swp, this);
        }

        private void OnInteractiveEdgesChanged(DependencyPropertyChangedEventArgs e)
        {
            CoerceValue(CursorProperty);
        }

        private object OnCoerceCursor(object baseValue)
        {
            Cursor cursor = (Cursor) baseValue;

            // Only coerce the default value.
            ValueSource vs = DependencyPropertyHelper.GetValueSource(this, CursorProperty);
            if (vs.BaseValueSource == BaseValueSource.Default)
                switch (InteractiveEdges)
                {
                    case MdiWindowEdge.None:
                        cursor = Cursors.Arrow;
                        break;

                    case MdiWindowEdge.Left:
                    case MdiWindowEdge.Right:
                        cursor = Cursors.SizeWE;
                        break;

                    case MdiWindowEdge.Top:
                    case MdiWindowEdge.Bottom:
                        cursor = Cursors.SizeNS;
                        break;

                    case MdiWindowEdge.Left | MdiWindowEdge.Top:
                    case MdiWindowEdge.Right | MdiWindowEdge.Bottom:
                        cursor = Cursors.SizeNWSE;
                        break;

                    case MdiWindowEdge.Left | MdiWindowEdge.Bottom:
                    case MdiWindowEdge.Right | MdiWindowEdge.Top:
                        cursor = Cursors.SizeNESW;
                        break;

                    default:
                        cursor = Cursors.Help;
                        break;
                }

            return cursor;
        }
    }
}
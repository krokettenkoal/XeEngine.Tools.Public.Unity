// MIT License
// 
// Copyright(c) 2018 Luciano (Xeeynamo) Ciccariello
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Part of this software belongs to XeEngine toolset and United Lines Studio
// and it is currently used to create commercial games by Luciano Ciccariello.
// Please do not redistribuite this code under your own name, stole it or use
// it artfully, but instead support it and its author. Thank you.

using System;
using System.Windows;
using System.Windows.Controls;

namespace Xe.Tools.Wpf.Controls
{
    public class TwoEqualColumnsPanel : Panel
    {
        public static readonly DependencyProperty ColumnSpacingProperty =
            DependencyProperty.Register(nameof(ColumnSpacing), typeof(double), typeof(TwoEqualColumnsPanel),
            new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty RowSpacingProperty =
            DependencyProperty.Register(nameof(RowSpacing), typeof(double), typeof(TwoEqualColumnsPanel),
            new FrameworkPropertyMetadata(0.0d, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double ColumnSpacing
        {
            get => (double)GetValue(ColumnSpacingProperty);
            set => SetValue(ColumnSpacingProperty, value);
        }

        public double RowSpacing
        {
            get => (double)GetValue(RowSpacingProperty);
            set => SetValue(RowSpacingProperty, value);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var y = 0.0;
            var width = (constraint.Width - ColumnSpacing) / 2.0;

            int count = VisualChildrenCount;
            for (var i = 0; i < count; i += 2)
            {
                var child1 = Children[i];
                var child3 = i + 1 < count ? Children[i + 1] : null;

                child1.Measure(new Size(width, constraint.Height));
                child3?.Measure(new Size(width, constraint.Height));

                var aboveHeight = Math.Max(child1.DesiredSize.Height, child3?.DesiredSize.Height ?? 0);

                y += aboveHeight + RowSpacing;
            }

            return new Size(constraint.Width, Math.Max(0, y - RowSpacing));
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var width = (arrangeSize.Width - ColumnSpacing) / 2.0;
            var rightY = (arrangeSize.Width + ColumnSpacing) / 2.0;
            var y = 0.0;

            int count = VisualChildrenCount;
            for (var i = 0; i < count; i += 2)
            {
                var child1 = Children[i];
                var child3 = i + 1 < count ? Children[i + 1] : null;

                var aboveHeight = Math.Max(child1.DesiredSize.Height, child3?.DesiredSize.Height ?? 0);

                child1.Arrange(new Rect(0, y, width, aboveHeight));
                child3?.Arrange(new Rect(rightY, y, width, aboveHeight));

                y += aboveHeight + RowSpacing;
            }

            return base.ArrangeOverride(arrangeSize);
        }

        private void InvalidateMeasureAndArrange()
        {
            InvalidateMeasure();
            InvalidateArrange();
        }
    }
}
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

using ScrollPresenter = Microsoft.UI.Xaml.Controls.Primitives.ScrollPresenter;
using ScrollView = Microsoft.UI.Xaml.Controls.ScrollView;
using ContentOrientation = Microsoft.UI.Xaml.Controls.ContentOrientation;
using ChainingMode = Microsoft.UI.Xaml.Controls.ChainingMode;
using RailingMode = Microsoft.UI.Xaml.Controls.RailingMode;
using ScrollMode = Microsoft.UI.Xaml.Controls.ScrollMode;
using ZoomMode = Microsoft.UI.Xaml.Controls.ZoomMode;
using InputKind = Microsoft.UI.Xaml.Controls.InputKind;
using ScrollBarVisibility = Microsoft.UI.Xaml.Controls.ScrollBarVisibility;
using ScrollingScrollAnimationStartingEventArgs = Microsoft.UI.Xaml.Controls.ScrollingScrollAnimationStartingEventArgs;
using ScrollingZoomAnimationStartingEventArgs = Microsoft.UI.Xaml.Controls.ScrollingZoomAnimationStartingEventArgs;
using MUXControlsTestHooks = Microsoft.UI.Private.Controls.MUXControlsTestHooks;
using MUXControlsTestHooksLoggingMessageEventArgs = Microsoft.UI.Private.Controls.MUXControlsTestHooksLoggingMessageEventArgs;
using ScrollViewTestHooks = Microsoft.UI.Private.Controls.ScrollViewTestHooks;

namespace MUXControlsTestApp
{
    public sealed partial class ScrollViewDynamicPage : TestPage
    {
        private Object asyncEventReportingLock = new Object();
        private List<string> lstAsyncEventMessage = new List<string>();
        private Image largeImg;
        private Image wuxLargeImg;
        private Rectangle rectangle = null;
        private Rectangle wuxRectangle = null;
        private Button button = null;
        private Button wuxButton = null;
        private Border border = null;
        private Border wuxBorder = null;
        private StackPanel sp1 = null;
        private StackPanel wuxSp1 = null;
        private StackPanel sp2 = null;
        private StackPanel wuxSp2 = null;
        private Viewbox viewbox = null;
        private Viewbox wuxViewbox = null;
        ScrollView scrollView = null;

        public ScrollViewDynamicPage()
        {
            this.InitializeComponent();
            CreateChildren();
            UseScrollView(this.markupScrollView);
        }

        private void ChkScrollViewProperties_Checked(object sender, RoutedEventArgs e)
        {
            if (grdScrollViewProperties != null)
                grdScrollViewProperties.Visibility = Visibility.Visible;
        }

        private void ChkScrollViewProperties_Unchecked(object sender, RoutedEventArgs e)
        {
            if (grdScrollViewProperties != null)
                grdScrollViewProperties.Visibility = Visibility.Collapsed;
        }

        private void ChkScrollPresenterClonedProperties_Checked(object sender, RoutedEventArgs e)
        {
            if (grdScrollPresenterClonedProperties != null)
                grdScrollPresenterClonedProperties.Visibility = Visibility.Visible;
        }

        private void ChkScrollPresenterClonedProperties_Unchecked(object sender, RoutedEventArgs e)
        {
            if (grdScrollPresenterClonedProperties != null)
                grdScrollPresenterClonedProperties.Visibility = Visibility.Collapsed;
        }

        private void ChkContentProperties_Checked(object sender, RoutedEventArgs e)
        {
            if (grdContentProperties != null)
                grdContentProperties.Visibility = Visibility.Visible;
        }

        private void ChkContentProperties_Unchecked(object sender, RoutedEventArgs e)
        {
            if (grdContentProperties != null)
                grdContentProperties.Visibility = Visibility.Collapsed;
        }

        private void ChkLogs_Checked(object sender, RoutedEventArgs e)
        {
            if (grdLogs != null)
                grdLogs.Visibility = Visibility.Visible;
        }

        private void ChkLogs_Unchecked(object sender, RoutedEventArgs e)
        {
            if (grdLogs != null)
                grdLogs.Visibility = Visibility.Collapsed;
        }

        private void BtnGetContentOrientation_Click(object sender, RoutedEventArgs e)
        {
            UpdateContentOrientation();
        }

        private void BtnGetHorizontalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateHorizontalScrollMode();
        }

        private void BtnGetHorizontalScrollChainingMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateHorizontalScrollChainingMode();
        }

        private void BtnGetHorizontalScrollRailingMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateHorizontalScrollRailingMode();
        }

        private void BtnGetVerticalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateVerticalScrollMode();
        }

        private void BtnGetVerticalScrollChainingMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateVerticalScrollChainingMode();
        }

        private void BtnGetVerticalScrollRailingMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateVerticalScrollRailingMode();
        }

        private void BtnGetZoomMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoomMode();
        }

        private void BtnGetZoomChainingMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoomChainingMode();
        }

        private void BtnGetIgnoredInputKind_Click(object sender, RoutedEventArgs e)
        {
            UpdateIgnoredInputKind();
        }

        private void BtnGetMinZoomFactor_Click(object sender, RoutedEventArgs e)
        {
            UpdateMinZoomFactor();
        }

        private void BtnGetMaxZoomFactor_Click(object sender, RoutedEventArgs e)
        {
            UpdateMaxZoomFactor();
        }

        private void BtnSetContentOrientation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ContentOrientation co = (ContentOrientation)cmbContentOrientation.SelectedIndex;
                scrollView.ContentOrientation = co;

                switch (co)
                {
                    case ContentOrientation.Horizontal:
                        wuxScrollViewer.HorizontalScrollBarVisibility = MuxScrollBarVisibilityToWuxScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                        wuxScrollViewer.VerticalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Disabled;
                        break;
                    case ContentOrientation.Vertical:
                        wuxScrollViewer.HorizontalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Disabled;
                        wuxScrollViewer.VerticalScrollBarVisibility = MuxScrollBarVisibilityToWuxScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                        break;
                    case ContentOrientation.None:
                        wuxScrollViewer.HorizontalScrollBarVisibility = MuxScrollBarVisibilityToWuxScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                        wuxScrollViewer.VerticalScrollBarVisibility = MuxScrollBarVisibilityToWuxScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                        break;
                    case ContentOrientation.Both:
                        wuxScrollViewer.HorizontalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Disabled;
                        wuxScrollViewer.VerticalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Disabled;
                        break;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetHorizontalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ScrollMode ssm = (ScrollMode)cmbHorizontalScrollMode.SelectedIndex;
                scrollView.HorizontalScrollMode = ssm;

                wuxScrollViewer.HorizontalScrollMode = MuxScrollModeToWuxScrollMode(ssm);
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetHorizontalScrollChainingMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChainingMode scm = (ChainingMode)cmbHorizontalScrollChainingMode.SelectedIndex;
                scrollView.HorizontalScrollChainingMode = scm;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetHorizontalScrollRailingMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RailingMode srm = (RailingMode)cmbHorizontalScrollRailingMode.SelectedIndex;
                scrollView.HorizontalScrollRailingMode = srm;

                wuxScrollViewer.IsHorizontalRailEnabled = MuxRailModeToWuxRailMode(srm);
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetVerticalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ScrollMode ssm = (ScrollMode)cmbVerticalScrollMode.SelectedIndex;
                scrollView.VerticalScrollMode = ssm;

                wuxScrollViewer.VerticalScrollMode = MuxScrollModeToWuxScrollMode(ssm);
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetVerticalScrollChainingMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChainingMode scm = (ChainingMode)cmbVerticalScrollChainingMode.SelectedIndex;
                scrollView.VerticalScrollChainingMode = scm;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetVerticalScrollRailingMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RailingMode srm = (RailingMode)cmbVerticalScrollRailingMode.SelectedIndex;
                scrollView.VerticalScrollRailingMode = srm;

                wuxScrollViewer.IsVerticalRailEnabled = MuxRailModeToWuxRailMode(srm);
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetZoomMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ZoomMode szm = (ZoomMode)cmbZoomMode.SelectedIndex;
                scrollView.ZoomMode = szm;

                wuxScrollViewer.ZoomMode = MuxZoomModeToWuxZoomMode(szm);
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetZoomChainingMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChainingMode scm = (ChainingMode)cmbZoomChainingMode.SelectedIndex;
                scrollView.ZoomChainingMode = scm;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetIgnoredInputKind_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InputKind ignoredInputKind;

                switch (cmbIgnoredInputKind.SelectedIndex)
                {
                    case 0:
                        ignoredInputKind = InputKind.None;
                        break;
                    case 1:
                        ignoredInputKind = InputKind.Touch;
                        break;
                    case 2:
                        ignoredInputKind = InputKind.Pen;
                        break;
                    case 3:
                        ignoredInputKind = InputKind.MouseWheel;
                        break;
                    case 4:
                        ignoredInputKind = InputKind.Keyboard;
                        break;
                    case 5:
                        ignoredInputKind = InputKind.Gamepad;
                        break;
                    default:
                        ignoredInputKind = InputKind.All;
                        break;
                }

                scrollView.IgnoredInputKind = ignoredInputKind;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetMinZoomFactor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.MinZoomFactor = Convert.ToDouble(txtMinZoomFactor.Text);

                wuxScrollViewer.MinZoomFactor = (float)scrollView.MinZoomFactor;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetMaxZoomFactor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.MaxZoomFactor = Convert.ToDouble(txtMaxZoomFactor.Text);

                wuxScrollViewer.MaxZoomFactor = (float)scrollView.MaxZoomFactor;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetHorizontalAnchorRatio_Click(object sender, RoutedEventArgs e)
        {
            UpdateHorizontalAnchorRatio();
        }

        private void BtnSetHorizontalAnchorRatio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.HorizontalAnchorRatio = Convert.ToDouble(txtHorizontalAnchorRatio.Text);

                wuxScrollViewer.HorizontalAnchorRatio = scrollView.HorizontalAnchorRatio;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetVerticalAnchorRatio_Click(object sender, RoutedEventArgs e)
        {
            UpdateVerticalAnchorRatio();
        }

        private void BtnSetVerticalAnchorRatio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.VerticalAnchorRatio = Convert.ToDouble(txtVerticalAnchorRatio.Text);

                wuxScrollViewer.VerticalAnchorRatio = scrollView.VerticalAnchorRatio;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetExtentWidth_Click(object sender, RoutedEventArgs e)
        {
            UpdateExtentWidth();
        }

        private void BtnGetExtentHeight_Click(object sender, RoutedEventArgs e)
        {
            UpdateExtentHeight();
        }

#if USE_SCROLLMODE_AUTO
        private void BtnGetComputedHorizontalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateTblComputedHorizontalScrollMode();
        }

        private void BtnGetComputedVerticalScrollMode_Click(object sender, RoutedEventArgs e)
        {
            UpdateTblComputedVerticalScrollMode();
        }

        private void UpdateTblComputedHorizontalScrollMode()
        { 
            try
            {
                tblComputedHorizontalScrollMode.Text = scrollView.ComputedHorizontalScrollMode.ToString();
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateTblComputedVerticalScrollMode()
        {
            try
            {
                tblComputedVerticalScrollMode.Text = scrollView.ComputedVerticalScrollMode.ToString();
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }
#endif

        private void UpdateCmbHorizontalScrollBarVisibility()
        {
            try
            {
                cmbHorizontalScrollBarVisibility.SelectedIndex = (int)scrollView.HorizontalScrollBarVisibility;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateCmbVerticalScrollBarVisibility()
        {
            try
            {
                cmbVerticalScrollBarVisibility.SelectedIndex = (int)scrollView.VerticalScrollBarVisibility;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateCmbXYFocusKeyboardNavigation()
        {
            try
            {
                cmbXYFocusKeyboardNavigation.SelectedIndex = (int)scrollView.XYFocusKeyboardNavigation;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateHorizontalAnchorRatio()
        {
            txtHorizontalAnchorRatio.Text = scrollView.HorizontalAnchorRatio.ToString();
        }

        private void UpdateVerticalAnchorRatio()
        {
            txtVerticalAnchorRatio.Text = scrollView.VerticalAnchorRatio.ToString();
        }

        private void UpdateExtentWidth()
        {
            txtExtentWidth.Text = scrollView.ExtentWidth.ToString();

            if (Math.Abs(scrollView.ExtentWidth - wuxScrollViewer.ExtentWidth / wuxScrollViewer.ZoomFactor) > 0.0001)
            {
                lstLogs.Items.Add($"muxScrollView.ExtentWidth={scrollView.ExtentWidth} != wuxScrollViewer.ExtentWidth/wuxScrollViewer.ZoomFactor={wuxScrollViewer.ExtentWidth / wuxScrollViewer.ZoomFactor}");
            }
        }

        private void UpdateExtentHeight()
        {
            txtExtentHeight.Text = scrollView.ExtentHeight.ToString();

            if (Math.Abs(scrollView.ExtentHeight - wuxScrollViewer.ExtentHeight / wuxScrollViewer.ZoomFactor) > 0.0001)
            {
                lstLogs.Items.Add($"muxScrollView.ExtentHeight={scrollView.ExtentHeight} != wuxScrollViewer.ExtentHeight/wuxScrollViewer.ZoomFactor={wuxScrollViewer.ExtentHeight / wuxScrollViewer.ZoomFactor}");
            }
        }

        private void BtnGetWidth_Click(object sender, RoutedEventArgs e)
        {
            UpdateWidth();
        }

        private void BtnSetWidth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.Width = Convert.ToDouble(txtWidth.Text);

                wuxScrollViewer.Width = scrollView.Width;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetHeight_Click(object sender, RoutedEventArgs e)
        {
            UpdateHeight();
        }

        private void BtnSetHeight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.Height = Convert.ToDouble(txtHeight.Text);

                wuxScrollViewer.Height = scrollView.Height;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void CmbBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbBackground.SelectedIndex)
            {
                case 0:
                    scrollView.Background = null;
                    break;
                case 1:
                    scrollView.Background = new SolidColorBrush(Colors.Transparent);
                    break;
                case 2:
                    scrollView.Background = new SolidColorBrush(Colors.AliceBlue);
                    break;
                case 3:
                    scrollView.Background = new SolidColorBrush(Colors.Aqua);
                    break;
            }

            wuxScrollViewer.Background = scrollView.Background;
        }

        private void CmbContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FrameworkElement currentContent = scrollView.Content as FrameworkElement;
                FrameworkElement newContent = null;
                FrameworkElement wuxNewContent = null;

                switch (cmbContent.SelectedIndex)
                {
                    case 1:
                        newContent = smallImg;
                        wuxNewContent = wuxSmallImg;
                        break;
                    case 2:
                        newContent = largeImg;
                        wuxNewContent = wuxLargeImg;
                        break;
                    case 3:
                        newContent = rectangle;
                        wuxNewContent = wuxRectangle;
                        break;
                    case 4:
                        newContent = button;
                        wuxNewContent = wuxButton;
                        break;
                    case 5:
                        newContent = border;
                        wuxNewContent = wuxBorder;
                        break;
                    case 6:
                        newContent = sp1;
                        wuxNewContent = wuxSp1;
                        break;
                    case 7:
                        newContent = sp2;
                        wuxNewContent = wuxSp2;
                        break;
                    case 8:
                        newContent = viewbox;
                        wuxNewContent = wuxViewbox;
                        break;
                }

                if (chkPreserveProperties.IsChecked == true && currentContent != null && newContent != null && wuxNewContent != null)
                {
                    newContent.MinWidth = currentContent.MinWidth;
                    newContent.Width = currentContent.Width;
                    newContent.MaxWidth = currentContent.MaxWidth;
                    newContent.MinHeight = currentContent.MinHeight;
                    newContent.Height = currentContent.Height;
                    newContent.MaxHeight = currentContent.MaxHeight;
                    newContent.Margin = currentContent.Margin;
                    newContent.HorizontalAlignment = currentContent.HorizontalAlignment;
                    newContent.VerticalAlignment = currentContent.VerticalAlignment;

                    if (currentContent is Control && newContent is Control)
                    {
                        ((Control)newContent).Padding = ((Control)currentContent).Padding;
                    }

                    wuxNewContent.MinWidth = currentContent.MinWidth;
                    wuxNewContent.Width = currentContent.Width;
                    wuxNewContent.MaxWidth = currentContent.MaxWidth;
                    wuxNewContent.MinHeight = currentContent.MinHeight;
                    wuxNewContent.Height = currentContent.Height;
                    wuxNewContent.MaxHeight = currentContent.MaxHeight;
                    wuxNewContent.Margin = currentContent.Margin;
                    wuxNewContent.HorizontalAlignment = currentContent.HorizontalAlignment;
                    wuxNewContent.VerticalAlignment = currentContent.VerticalAlignment;

                    if (currentContent is Control && wuxNewContent is Control)
                    {
                        ((Control)wuxNewContent).Padding = ((Control)currentContent).Padding;
                    }
                }

                scrollView.Content = newContent;

                wuxScrollViewer.Content = wuxNewContent;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());

                UpdateContent();
            }
        }

        private void CmbHorizontalScrollBarVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                scrollView.HorizontalScrollBarVisibility = (ScrollBarVisibility)cmbHorizontalScrollBarVisibility.SelectedIndex;

                ContentOrientation co = (ContentOrientation)cmbContentOrientation.SelectedIndex;
                switch (co)
                {
                    case ContentOrientation.Vertical:
                    case ContentOrientation.Both:
                        wuxScrollViewer.HorizontalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Disabled;
                        break;
                    default:
                        wuxScrollViewer.HorizontalScrollBarVisibility = MuxScrollBarVisibilityToWuxScrollBarVisibility(scrollView.HorizontalScrollBarVisibility);
                        break;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void CmbVerticalScrollBarVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                scrollView.VerticalScrollBarVisibility = (ScrollBarVisibility)cmbVerticalScrollBarVisibility.SelectedIndex;

                ContentOrientation co = (ContentOrientation)cmbContentOrientation.SelectedIndex;
                switch (co)
                {
                    case ContentOrientation.Horizontal:
                    case ContentOrientation.Both:
                        wuxScrollViewer.VerticalScrollBarVisibility = Windows.UI.Xaml.Controls.ScrollBarVisibility.Disabled;
                        break;
                    default:
                        wuxScrollViewer.VerticalScrollBarVisibility = MuxScrollBarVisibilityToWuxScrollBarVisibility(scrollView.VerticalScrollBarVisibility);
                        break;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void CmbXYFocusKeyboardNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                scrollView.XYFocusKeyboardNavigation = (XYFocusKeyboardNavigationMode)cmbXYFocusKeyboardNavigation.SelectedIndex;

                wuxScrollViewer.XYFocusKeyboardNavigation = scrollView.XYFocusKeyboardNavigation;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetMargin_Click(object sender, RoutedEventArgs e)
        {
            UpdateMargin();
        }

        private void BtnSetMargin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.Margin = GetThicknessFromString(txtMargin.Text);

                wuxScrollViewer.Margin = scrollView.Margin;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetPadding_Click(object sender, RoutedEventArgs e)
        {
            UpdatePadding();
        }

        private void BtnSetPadding_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                scrollView.Padding = GetThicknessFromString(txtPadding.Text);

                wuxScrollViewer.Padding = scrollView.Padding;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void ChkIsEnabled_Checked(object sender, RoutedEventArgs e)
        {
            scrollView.IsEnabled = true;

            wuxScrollViewer.IsEnabled = true;
        }

        private void ChkIsEnabled_Unchecked(object sender, RoutedEventArgs e)
        {
            scrollView.IsEnabled = false;

            wuxScrollViewer.IsEnabled = false;
        }

        private void ChkIsTabStop_Checked(object sender, RoutedEventArgs e)
        {
            scrollView.IsTabStop = true;

            wuxScrollViewer.IsTabStop = true;
        }

        private void ChkIsTabStop_Unchecked(object sender, RoutedEventArgs e)
        {
            scrollView.IsTabStop = false;

            wuxScrollViewer.IsTabStop = false;
        }

        private void BtnGetContentMinWidth_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentMinWidth.Text = string.Empty;
            else
                txtContentMinWidth.Text = ((FrameworkElement)scrollView.Content).MinWidth.ToString();
        }

        private void BtnGetContentWidth_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentWidth.Text = string.Empty;
            else
                txtContentWidth.Text = ((FrameworkElement)scrollView.Content).Width.ToString();
        }

        private void BtnGetContentMaxWidth_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentMaxWidth.Text = string.Empty;
            else
                txtContentMaxWidth.Text = ((FrameworkElement)scrollView.Content).MaxWidth.ToString();
        }

        private void BtnSetContentMinWidth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double minWidth = Convert.ToDouble(txtContentMinWidth.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).MinWidth = minWidth;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).MinWidth = minWidth;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetContentWidth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double width = Convert.ToDouble(txtContentWidth.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).Width = width;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).Width = width;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetContentMaxWidth_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double maxWidth = Convert.ToDouble(txtContentMaxWidth.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).MaxWidth = maxWidth;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).MaxWidth = maxWidth;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetContentMinHeight_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentMinHeight.Text = string.Empty;
            else
                txtContentMinHeight.Text = ((FrameworkElement)scrollView.Content).MinHeight.ToString();
        }

        private void BtnGetContentHeight_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentHeight.Text = string.Empty;
            else
                txtContentHeight.Text = ((FrameworkElement)scrollView.Content).Height.ToString();
        }

        private void BtnGetContentMaxHeight_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentMaxHeight.Text = string.Empty;
            else
                txtContentMaxHeight.Text = ((FrameworkElement)scrollView.Content).MaxHeight.ToString();
        }

        private void BtnSetContentMinHeight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double minHeight = Convert.ToDouble(txtContentMinHeight.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).MinHeight = minHeight;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).MinHeight = minHeight;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetContentHeight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double height = Convert.ToDouble(txtContentHeight.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).Height = height;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).Height = height;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnSetContentMaxHeight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double maxHeight = Convert.ToDouble(txtContentMaxHeight.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).MaxHeight = maxHeight;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).MaxHeight = maxHeight;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetContentMargin_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentMargin.Text = string.Empty;
            else
                txtContentMargin.Text = ((FrameworkElement)scrollView.Content).Margin.ToString();
        }

        private void BtnSetContentMargin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thickness margin = GetThicknessFromString(txtContentMargin.Text);

                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).Margin = margin;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    ((FrameworkElement)wuxScrollViewer.Content).Margin = margin;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetContentPadding_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is Control || scrollView.Content is Border || scrollView.Content is StackPanel))
                txtContentPadding.Text = string.Empty;
            else if (scrollView.Content is Control)
                txtContentPadding.Text = ((Control)scrollView.Content).Padding.ToString();
            else if (scrollView.Content is Border)
                txtContentPadding.Text = ((Border)scrollView.Content).Padding.ToString();
            else
                txtContentPadding.Text = ((StackPanel)scrollView.Content).Padding.ToString();
        }

        private void BtnSetContentPadding_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thickness padding = GetThicknessFromString(txtContentPadding.Text);

                if (scrollView.Content is Control)
                {
                    ((Control)scrollView.Content).Padding = padding;
                }
                else if (scrollView.Content is Border)
                {
                    ((Border)scrollView.Content).Padding = padding;
                }
                else if (scrollView.Content is StackPanel)
                {
                    ((StackPanel)scrollView.Content).Padding = padding;
                }

                if (wuxScrollViewer.Content is Control)
                {
                    ((Control)wuxScrollViewer.Content).Padding = padding;
                }
                else if (wuxScrollViewer.Content is Border)
                {
                    ((Border)wuxScrollViewer.Content).Padding = padding;
                }
                else if (wuxScrollViewer.Content is StackPanel)
                {
                    ((StackPanel)wuxScrollViewer.Content).Padding = padding;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void BtnGetContentActualWidth_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentActualWidth.Text = string.Empty;
            else
            {
                txtContentActualWidth.Text = (scrollView.Content as FrameworkElement).ActualWidth.ToString();

                if ((scrollView.Content as FrameworkElement).ActualWidth != (wuxScrollViewer.Content as FrameworkElement).ActualWidth)
                {
                    lstLogs.Items.Add($"muxScrollView.Content.ActualWidth={(scrollView.Content as FrameworkElement).ActualWidth} != wuxScrollViewer.Content.ActualWidth={(wuxScrollViewer.Content as FrameworkElement).ActualWidth}");
                }
            }
        }

        private void BtnGetContentActualHeight_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null || !(scrollView.Content is FrameworkElement))
                txtContentActualHeight.Text = string.Empty;
            else
            {
                txtContentActualHeight.Text = (scrollView.Content as FrameworkElement).ActualHeight.ToString();

                if ((scrollView.Content as FrameworkElement).ActualHeight != (wuxScrollViewer.Content as FrameworkElement).ActualHeight)
                {
                    lstLogs.Items.Add($"muxScrollView.Content.ActualHeight={(scrollView.Content as FrameworkElement).ActualHeight} != wuxScrollViewer.Content.ActualHeight={(wuxScrollViewer.Content as FrameworkElement).ActualHeight}");
                }
            }
        }

        private void BtnGetContentDesiredSize_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null)
                txtContentDesiredSize.Text = string.Empty;
            else
            {
                txtContentDesiredSize.Text = scrollView.Content.DesiredSize.ToString();

                if (scrollView.Content.DesiredSize != (wuxScrollViewer.Content as UIElement).DesiredSize)
                {
                    lstLogs.Items.Add($"muxScrollView.Content.DesiredSize={scrollView.Content.DesiredSize} != wuxScrollViewer.Content.DesiredSize={(wuxScrollViewer.Content as UIElement).DesiredSize}");
                }
            }
        }

        private void BtnGetContentRenderSize_Click(object sender, RoutedEventArgs e)
        {
            if (scrollView.Content == null)
                txtContentRenderSize.Text = string.Empty;
            else
            {
                txtContentRenderSize.Text = scrollView.Content.RenderSize.ToString();

                if (scrollView.Content.RenderSize != (wuxScrollViewer.Content as UIElement).RenderSize)
                {
                    lstLogs.Items.Add($"muxScrollView.Content.RenderSize={scrollView.Content.RenderSize} != wuxScrollViewer.Content.RenderSize={(wuxScrollViewer.Content as UIElement).RenderSize}");
                }
            }
        }

        private void CmbContentHorizontalAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).HorizontalAlignment = (HorizontalAlignment)cmbContentHorizontalAlignment.SelectedIndex;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    wuxScrollViewer.HorizontalContentAlignment =
                    ((FrameworkElement)wuxScrollViewer.Content).HorizontalAlignment = (HorizontalAlignment)cmbContentHorizontalAlignment.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void CmbContentVerticalAlignment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (scrollView.Content is FrameworkElement)
                {
                    ((FrameworkElement)scrollView.Content).VerticalAlignment = (VerticalAlignment)cmbContentVerticalAlignment.SelectedIndex;
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    wuxScrollViewer.VerticalContentAlignment =
                    ((FrameworkElement)wuxScrollViewer.Content).VerticalAlignment = (VerticalAlignment)cmbContentVerticalAlignment.SelectedIndex;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void CmbContentManipulationMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (scrollView.Content is FrameworkElement)
                {
                    switch (cmbContentManipulationMode.SelectedIndex)
                    {
                        case 0:
                            scrollView.Content.ManipulationMode = ManipulationModes.System;
                            break;
                        case 1:
                            scrollView.Content.ManipulationMode = ManipulationModes.None;
                            break;
                    }
                }

                if (wuxScrollViewer.Content is FrameworkElement)
                {
                    switch (cmbContentManipulationMode.SelectedIndex)
                    {
                        case 0:
                            (wuxScrollViewer.Content as FrameworkElement).ManipulationMode = ManipulationModes.System;
                            break;
                        case 1:
                            (wuxScrollViewer.Content as FrameworkElement).ManipulationMode = ManipulationModes.None;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateCmbContentHorizontalAlignment()
        {
            try
            {
                if (scrollView.Content is FrameworkElement)
                {
                    cmbContentHorizontalAlignment.SelectedIndex = (int)((FrameworkElement)scrollView.Content).HorizontalAlignment;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateCmbContentVerticalAlignment()
        {
            try
            {
                if (scrollView.Content is FrameworkElement)
                {
                    cmbContentVerticalAlignment.SelectedIndex = (int)((FrameworkElement)scrollView.Content).VerticalAlignment;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateCmbContentManipulationMode()
        {
            try
            {
                if (scrollView.Content != null)
                {
                    switch (scrollView.Content.ManipulationMode)
                    {
                        case ManipulationModes.System:
                            cmbContentManipulationMode.SelectedIndex = 0;
                            break;
                        case ManipulationModes.None:
                            cmbContentManipulationMode.SelectedIndex = 1;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateContentOrientation()
        {
            try
            {
                cmbContentOrientation.SelectedIndex = (int)scrollView.ContentOrientation;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateHorizontalScrollMode()
        {
            try
            {
                cmbHorizontalScrollMode.SelectedIndex = (int)scrollView.HorizontalScrollMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateHorizontalScrollChainingMode()
        {
            try
            {
                cmbHorizontalScrollChainingMode.SelectedIndex = (int)scrollView.HorizontalScrollChainingMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateHorizontalScrollRailingMode()
        {
            try
            {
                cmbHorizontalScrollRailingMode.SelectedIndex = (int)scrollView.HorizontalScrollRailingMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateVerticalScrollMode()
        {
            try
            {
                cmbVerticalScrollMode.SelectedIndex = (int)scrollView.VerticalScrollMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateVerticalScrollChainingMode()
        {
            try
            {
                cmbVerticalScrollChainingMode.SelectedIndex = (int)scrollView.VerticalScrollChainingMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateVerticalScrollRailingMode()
        {
            try
            {
                cmbVerticalScrollRailingMode.SelectedIndex = (int)scrollView.VerticalScrollRailingMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateZoomMode()
        {
            try
            {
                cmbZoomMode.SelectedIndex = (int)scrollView.ZoomMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateMinZoomFactor()
        {
            try
            {
                txtMinZoomFactor.Text = scrollView.MinZoomFactor.ToString();
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateMaxZoomFactor()
        {
            try
            {
                txtMaxZoomFactor.Text = scrollView.MaxZoomFactor.ToString();
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateZoomChainingMode()
        {
            try
            {
                cmbZoomChainingMode.SelectedIndex = (int)scrollView.ZoomChainingMode;
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateIgnoredInputKind()
        {
            try
            {
                switch (scrollView.IgnoredInputKind)
                {
                    case InputKind.None:
                        cmbIgnoredInputKind.SelectedIndex = 0;
                        break;
                    case InputKind.Touch:
                        cmbIgnoredInputKind.SelectedIndex = 1;
                        break;
                    case InputKind.Pen:
                        cmbIgnoredInputKind.SelectedIndex = 2;
                        break;
                    case InputKind.MouseWheel:
                        cmbIgnoredInputKind.SelectedIndex = 3;
                        break;
                    case InputKind.Keyboard:
                        cmbIgnoredInputKind.SelectedIndex = 4;
                        break;
                    case InputKind.Gamepad:
                        cmbIgnoredInputKind.SelectedIndex = 5;
                        break;
                    case InputKind.All:
                        cmbIgnoredInputKind.SelectedIndex = 6;
                        break;
                    default:
                        lstLogs.Items.Add("Unexpected IgnoredInputKind value.");
                        break;
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private void UpdateWidth()
        {
            txtWidth.Text = scrollView.Width.ToString();
        }

        private void UpdateHeight()
        {
            txtHeight.Text = scrollView.Height.ToString();
        }

        private void UpdateBackground()
        {
            if (scrollView.Background == null)
            {
                cmbBackground.SelectedIndex = 0;
            }
            else if ((scrollView.Background as SolidColorBrush).Color == Colors.Transparent)
            {
                cmbBackground.SelectedIndex = 1;
            }
            else if ((scrollView.Background as SolidColorBrush).Color == Colors.AliceBlue)
            {
                cmbBackground.SelectedIndex = 2;
            }
            else if ((scrollView.Background as SolidColorBrush).Color == Colors.Aqua)
            {
                cmbBackground.SelectedIndex = 3;
            }
        }

        private void UpdateContent()
        {
            if (scrollView.Content == null)
            {
                cmbContent.SelectedIndex = 0;
            }
            else if (scrollView.Content is Image)
            {
                if (((scrollView.Content as Image).Source as BitmapImage).UriSource.AbsolutePath.ToLower().Contains("large"))
                {
                    cmbContent.SelectedIndex = 2;
                }
                else
                {
                    cmbContent.SelectedIndex = 1;
                }
            }
            else if (scrollView.Content is Rectangle)
            {
                cmbContent.SelectedIndex = 3;
            }
            else if (scrollView.Content is Button)
            {
                cmbContent.SelectedIndex = 4;
            }
            else if (scrollView.Content is Border)
            {
                cmbContent.SelectedIndex = 5;
            }
            else if (scrollView.Content is StackPanel)
            {
                if ((scrollView.Content as StackPanel).Children.Count == 2)
                {
                    cmbContent.SelectedIndex = 6;
                }
                else
                {
                    cmbContent.SelectedIndex = 7;
                }
            }
            else if (scrollView.Content is Viewbox)
            {
                cmbContent.SelectedIndex = 8;
            }
        }

        private void UpdateMargin()
        {
            txtMargin.Text = scrollView.Margin.ToString();
        }

        private void UpdatePadding()
        {
            txtPadding.Text = scrollView.Padding.ToString();
        }

        private void CreateChildren()
        {
            largeImg = new Image() { Source = new BitmapImage(new Uri("ms-appx:/Assets/LargeWisteria.jpg")) };
            LinearGradientBrush lgb = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(1, 1) };
            GradientStop gs = new GradientStop() { Color = Colors.Blue, Offset = 0.0 };
            lgb.GradientStops.Add(gs);
            gs = new GradientStop() { Color = Colors.White, Offset = 0.5 };
            lgb.GradientStops.Add(gs);
            gs = new GradientStop() { Color = Colors.Red, Offset = 1.0 };
            lgb.GradientStops.Add(gs);
            rectangle = new Rectangle() { Fill = lgb };
            rectangle.Name = "rect";
            button = new Button() { Content = "Button" };
            button.Name = "btn";
            Rectangle borderChild = new Rectangle() { Fill = lgb };
            border = new Border() { BorderBrush = new SolidColorBrush(Colors.Chartreuse), BorderThickness = new Thickness(5), Child = borderChild };
            border.Name = "bdr";

            sp1 = new StackPanel();
            sp1.Name = "sp1";
            Button button1 = new Button() { Content = "Button1" };
            button1.Name = "btn1";
            button1.Margin = new Thickness(50);
            Button button2 = new Button() { Content = "Button2" };
            button2.Name = "btn2";
            button2.Margin = new Thickness(50);
            sp1.Children.Add(button1);
            sp1.Children.Add(button2);

            sp2 = new StackPanel();
            sp2.Name = "sp2";
            sp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Indigo) });
            sp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Orange) });
            sp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Purple) });
            sp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Goldenrod) });

            viewbox = new Viewbox();
            viewbox.Name = "viewbox";
            Rectangle viewboxChild = new Rectangle() { Fill = lgb, Width = 600, Height = 400 };
            viewbox.Child = viewboxChild;

            wuxLargeImg = new Image() { Source = new BitmapImage(new Uri("ms-appx:/Assets/LargeWisteria.jpg")) };
            wuxRectangle = new Rectangle() { Fill = lgb };
            wuxRectangle.Name = "wuxRect";
            wuxButton = new Button() { Content = "Button" };
            wuxButton.Name = "wuxBtn";
            borderChild = new Rectangle() { Fill = lgb };
            wuxBorder = new Border() { BorderBrush = new SolidColorBrush(Colors.Chartreuse), BorderThickness = new Thickness(5), Child = borderChild };
            wuxBorder.Name = "wuxBdr";

            wuxSp1 = new StackPanel();
            wuxSp1.Name = "wuxSp1";
            button1 = new Button() { Content = "Button1" };
            button1.Name = "wuxBtn1";
            button1.Margin = new Thickness(50);
            button2 = new Button() { Content = "Button2" };
            button2.Name = "wuxBtn2";
            button2.Margin = new Thickness(50);
            wuxSp1.Children.Add(button1);
            wuxSp1.Children.Add(button2);

            wuxSp2 = new StackPanel();
            wuxSp2.Name = "wuxSp2";
            wuxSp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Indigo) });
            wuxSp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Orange) });
            wuxSp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Purple) });
            wuxSp2.Children.Add(new Rectangle() { Height = 200, Fill = new SolidColorBrush(Colors.Goldenrod) });

            wuxViewbox = new Viewbox();
            wuxViewbox.Name = "wuxViewbox";
            viewboxChild = new Rectangle() { Fill = lgb, Width = 600, Height = 400 };
            wuxViewbox.Child = viewboxChild;
        }

        private Thickness GetThicknessFromString(string thickness)
        {
            string[] lengths = thickness.Split(',');
            if (lengths.Length < 4)
                return new Thickness(Convert.ToDouble(lengths[0]));
            else
                return new Thickness(
                    Convert.ToDouble(lengths[0]), Convert.ToDouble(lengths[1]), Convert.ToDouble(lengths[2]), Convert.ToDouble(lengths[3]));
        }

        private void UseScrollView(ScrollView sv2)
        {
            if (scrollView == sv2 || sv2 == null)
                return;

            try
            {
                if (scrollView == null && (chkLogScrollViewMessages.IsChecked == true || chkLogScrollPresenterMessages.IsChecked == true))
                {
                    MUXControlsTestHooks.LoggingMessage += MUXControlsTestHooks_LoggingMessage;

                    if (chkLogScrollPresenterMessages.IsChecked == true)
                    {
                        MUXControlsTestHooks.SetLoggingLevelForType("ScrollPresenter", isLoggingInfoLevel: true, isLoggingVerboseLevel: true);
                    }
                    if (chkLogScrollViewMessages.IsChecked == true)
                    {
                        MUXControlsTestHooks.SetLoggingLevelForType("ScrollView", isLoggingInfoLevel: true, isLoggingVerboseLevel: true);
                    }
                }

                if (scrollView != null)
                {
                    scrollView.GettingFocus -= ScrollView_GettingFocus;
                    scrollView.GotFocus -= ScrollView_GotFocus;
                    scrollView.LosingFocus -= ScrollView_LosingFocus;
                    scrollView.LostFocus -= ScrollView_LostFocus;

                    if (chkLogScrollViewEvents.IsChecked == true)
                    {
                        scrollView.ExtentChanged -= ScrollView_ExtentChanged;
                        scrollView.StateChanged -= ScrollView_StateChanged;
                        scrollView.ViewChanged -= ScrollView_ViewChanged;
                        scrollView.ScrollAnimationStarting -= ScrollView_ScrollAnimationStarting;
                        scrollView.ZoomAnimationStarting -= ScrollView_ZoomAnimationStarting;
                    }

                    ScrollPresenter scrollPresenter = ScrollViewTestHooks.GetScrollPresenterPart(scrollView);

                    if (scrollPresenter != null && chkLogScrollPresenterEvents.IsChecked == true)
                    {
                        scrollPresenter.ExtentChanged -= ScrollPresenter_ExtentChanged;
                        scrollPresenter.StateChanged -= ScrollPresenter_StateChanged;
                        scrollPresenter.ViewChanged -= ScrollPresenter_ViewChanged;
                        scrollPresenter.ScrollAnimationStarting -= ScrollPresenter_ScrollAnimationStarting;
                        scrollPresenter.ZoomAnimationStarting -= ScrollPresenter_ZoomAnimationStarting;
                    }
                }

                scrollView = sv2;

                UpdateContentOrientation();
                UpdateHorizontalScrollMode();
                UpdateHorizontalScrollChainingMode();
                UpdateHorizontalScrollRailingMode();
                UpdateVerticalScrollMode();
                UpdateVerticalScrollChainingMode();
                UpdateVerticalScrollRailingMode();
                UpdateZoomMode();
                UpdateZoomChainingMode();
                UpdateIgnoredInputKind();
                UpdateMinZoomFactor();
                UpdateMaxZoomFactor();

                UpdateWidth();
                UpdateHeight();
                UpdateBackground();
                UpdateContent();
                UpdateMargin();
                UpdatePadding();

                UpdateCmbHorizontalScrollBarVisibility();
                UpdateCmbVerticalScrollBarVisibility();
                UpdateCmbXYFocusKeyboardNavigation();
                UpdateHorizontalAnchorRatio();
                UpdateVerticalAnchorRatio();
                UpdateExtentWidth();
                UpdateExtentHeight();
#if USE_SCROLLMODE_AUTO
                UpdateTblComputedHorizontalScrollMode();
                UpdateTblComputedVerticalScrollMode();
#endif
                UpdateCmbContentHorizontalAlignment();
                UpdateCmbContentVerticalAlignment();
                UpdateCmbContentManipulationMode();

                chkIsEnabled.IsChecked = scrollView.IsEnabled;
                chkIsTabStop.IsChecked = scrollView.IsTabStop;

                if (scrollView != null)
                {
                    scrollView.GettingFocus += ScrollView_GettingFocus;
                    scrollView.GotFocus += ScrollView_GotFocus;
                    scrollView.LosingFocus += ScrollView_LosingFocus;
                    scrollView.LostFocus += ScrollView_LostFocus;

                    if (chkLogScrollViewEvents.IsChecked == true)
                    {
                        scrollView.ExtentChanged += ScrollView_ExtentChanged;
                        scrollView.StateChanged += ScrollView_StateChanged;
                        scrollView.ViewChanged += ScrollView_ViewChanged;
                        scrollView.ScrollAnimationStarting += ScrollView_ScrollAnimationStarting;
                        scrollView.ZoomAnimationStarting += ScrollView_ZoomAnimationStarting;
                    }

                    ScrollPresenter scrollPresenter = ScrollViewTestHooks.GetScrollPresenterPart(scrollView);

                    if (scrollPresenter != null && chkLogScrollPresenterEvents.IsChecked == true)
                    {
                        scrollPresenter.ExtentChanged += ScrollPresenter_ExtentChanged;
                        scrollPresenter.StateChanged += ScrollPresenter_StateChanged;
                        scrollPresenter.ViewChanged += ScrollPresenter_ViewChanged;
                        scrollPresenter.ScrollAnimationStarting += ScrollPresenter_ScrollAnimationStarting;
                        scrollPresenter.ZoomAnimationStarting += ScrollPresenter_ZoomAnimationStarting;
                    }
                }
            }
            catch (Exception ex)
            {
                txtExceptionReport.Text = ex.ToString();
                lstLogs.Items.Add(ex.ToString());
            }
        }

        private Windows.UI.Xaml.Controls.ScrollBarVisibility MuxScrollBarVisibilityToWuxScrollBarVisibility(ScrollBarVisibility muxScrollBarVisibility)
        {
            switch (muxScrollBarVisibility)
            {
                case ScrollBarVisibility.Auto:
                    return Windows.UI.Xaml.Controls.ScrollBarVisibility.Auto;
                case ScrollBarVisibility.Hidden:
                    return Windows.UI.Xaml.Controls.ScrollBarVisibility.Hidden;
                default:
                    return Windows.UI.Xaml.Controls.ScrollBarVisibility.Visible;
            }
        }

        private Windows.UI.Xaml.Controls.ScrollMode MuxScrollModeToWuxScrollMode(ScrollMode muxScrollMode)
        {
            switch (muxScrollMode)
            {
                case ScrollMode.Disabled:
                    return Windows.UI.Xaml.Controls.ScrollMode.Disabled;
                default:
                    return Windows.UI.Xaml.Controls.ScrollMode.Enabled;
            }
        }

        private Windows.UI.Xaml.Controls.ZoomMode MuxZoomModeToWuxZoomMode(ZoomMode muxZoomMode)
        {
            switch (muxZoomMode)
            {
                case ZoomMode.Disabled:
                    return Windows.UI.Xaml.Controls.ZoomMode.Disabled;
                default:
                    return Windows.UI.Xaml.Controls.ZoomMode.Enabled;
            }
        }

        private bool MuxRailModeToWuxRailMode(RailingMode muxRailingMode)
        {
            switch (muxRailingMode)
            {
                case RailingMode.Disabled:
                    return false;
                default:
                    return true;
            }
        }

        private void ScrollView_GettingFocus(UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            FrameworkElement oldFE = args.OldFocusedElement as FrameworkElement;
            string oldFEName = (oldFE == null) ? "null" : oldFE.Name;
            FrameworkElement newFE = args.NewFocusedElement as FrameworkElement;
            string newFEName = (newFE == null) ? "null" : newFE.Name;

            AppendAsyncEventMessage("ScrollView.GettingFocus FocusState=" + args.FocusState + ", Direction=" + args.Direction + ", InputDevice=" + args.InputDevice + ", oldFE=" + oldFEName + ", newFE=" + newFEName);
        }

        private void ScrollView_LostFocus(object sender, RoutedEventArgs e)
        {
            AppendAsyncEventMessage("ScrollView.LostFocus");
        }

        private void ScrollView_LosingFocus(UIElement sender, Windows.UI.Xaml.Input.LosingFocusEventArgs args)
        {
            FrameworkElement oldFE = args.OldFocusedElement as FrameworkElement;
            string oldFEName = (oldFE == null) ? "null" : oldFE.Name;
            FrameworkElement newFE = args.NewFocusedElement as FrameworkElement;
            string newFEName = (newFE == null) ? "null" : newFE.Name;

            AppendAsyncEventMessage("ScrollView.LosingFocus FocusState=" + args.FocusState + ", Direction=" + args.Direction + ", InputDevice=" + args.InputDevice + ", oldFE=" + oldFEName + ", newFE=" + newFEName);
        }

        private void ScrollView_GotFocus(object sender, RoutedEventArgs e)
        {
            AppendAsyncEventMessage("ScrollView.GotFocus");
        }

        private void ScrollPresenter_ExtentChanged(ScrollPresenter sender, object args)
        {
            AppendAsyncEventMessage("ScrollPresenter.ExtentChanged ExtentWidth=" + sender.ExtentWidth + ", ExtentHeight=" + sender.ExtentHeight);
        }

        private void ScrollPresenter_StateChanged(ScrollPresenter sender, object args)
        {
            AppendAsyncEventMessage("ScrollPresenter.StateChanged " + sender.State.ToString());
        }

        private void ScrollPresenter_ViewChanged(ScrollPresenter sender, object args)
        {
            AppendAsyncEventMessage("ScrollPresenter.ViewChanged H=" + sender.HorizontalOffset.ToString() + ", V=" + sender.VerticalOffset + ", ZF=" + sender.ZoomFactor);
        }

        private void ScrollPresenter_ScrollAnimationStarting(ScrollPresenter sender, ScrollingScrollAnimationStartingEventArgs args)
        {
            AppendAsyncEventMessage("ScrollPresenter.ScrollAnimationStarting OffsetsChangeId=" + args.ScrollInfo.OffsetsChangeId + " SP=(" + args.StartPosition.X + "," + args.StartPosition.Y +") EP=(" + args.EndPosition.X + "," + args.EndPosition.Y +")");
        }

        private void ScrollPresenter_ZoomAnimationStarting(ScrollPresenter sender, ScrollingZoomAnimationStartingEventArgs args)
        {
            AppendAsyncEventMessage("ScrollPresenter.ZoomAnimationStarting ZoomFactorChangeId=" + args.ZoomInfo.ZoomFactorChangeId + ", CenterPoint=" + args.CenterPoint + ", SZF=" + args.StartZoomFactor + ", EZF=" + args.EndZoomFactor);
        }

        private void ScrollView_ExtentChanged(ScrollView sender, object args)
        {
            AppendAsyncEventMessage("ScrollView.ExtentChanged ExtentWidth=" + sender.ExtentWidth + ", ExtentHeight=" + sender.ExtentHeight);
        }

        private void ScrollView_StateChanged(ScrollView sender, object args)
        {
            AppendAsyncEventMessage("ScrollView.StateChanged " + sender.State.ToString());
        }

        private void ScrollView_ViewChanged(ScrollView sender, object args)
        {
            AppendAsyncEventMessage("ScrollView.ViewChanged H=" + sender.HorizontalOffset.ToString() + ", V=" + sender.VerticalOffset + ", ZF=" + sender.ZoomFactor);
        }

        private void ScrollView_ScrollAnimationStarting(ScrollView sender, ScrollingScrollAnimationStartingEventArgs args)
        {
            AppendAsyncEventMessage("ScrollView.ScrollAnimationStarting OffsetsChangeId=" + args.ScrollInfo.OffsetsChangeId);
        }

        private void ScrollView_ZoomAnimationStarting(ScrollView sender, ScrollingZoomAnimationStartingEventArgs args)
        {
            AppendAsyncEventMessage("ScrollView.ZoomAnimationStarting ZoomFactorChangeId=" + args.ZoomInfo.ZoomFactorChangeId + ", CenterPoint=" + args.CenterPoint);
        }

        private void BtnClearLogs_Click(object sender, RoutedEventArgs e)
        {
            lstLogs.Items.Clear();
        }

        private void ChkLogScrollPresenterEvents_Checked(object sender, RoutedEventArgs e)
        {
            if (scrollView != null)
            {
                ScrollPresenter scrollPresenter = ScrollViewTestHooks.GetScrollPresenterPart(scrollView);

                if (scrollPresenter != null)
                {
                    scrollPresenter.ExtentChanged += ScrollPresenter_ExtentChanged;
                    scrollPresenter.StateChanged += ScrollPresenter_StateChanged;
                    scrollPresenter.ViewChanged += ScrollPresenter_ViewChanged;
                    scrollPresenter.ScrollAnimationStarting += ScrollPresenter_ScrollAnimationStarting;
                    scrollPresenter.ZoomAnimationStarting += ScrollPresenter_ZoomAnimationStarting;
                }
            }
        }

        private void ChkLogScrollPresenterEvents_Unchecked(object sender, RoutedEventArgs e)
        {
            if (scrollView != null)
            {
                ScrollPresenter scrollPresenter = ScrollViewTestHooks.GetScrollPresenterPart(scrollView);

                if (scrollPresenter != null)
                {
                    scrollPresenter.ExtentChanged -= ScrollPresenter_ExtentChanged;
                    scrollPresenter.StateChanged -= ScrollPresenter_StateChanged;
                    scrollPresenter.ViewChanged -= ScrollPresenter_ViewChanged;
                    scrollPresenter.ScrollAnimationStarting -= ScrollPresenter_ScrollAnimationStarting;
                    scrollPresenter.ZoomAnimationStarting -= ScrollPresenter_ZoomAnimationStarting;
                }
            }
        }

        private void ChkLogScrollViewEvents_Checked(object sender, RoutedEventArgs e)
        {
            if (scrollView != null)
            {
                scrollView.ExtentChanged += ScrollView_ExtentChanged;
                scrollView.StateChanged += ScrollView_StateChanged;
                scrollView.ViewChanged += ScrollView_ViewChanged;
                scrollView.ScrollAnimationStarting += ScrollView_ScrollAnimationStarting;
                scrollView.ZoomAnimationStarting += ScrollView_ZoomAnimationStarting;
            }
        }

        private void ChkLogScrollViewEvents_Unchecked(object sender, RoutedEventArgs e)
        {
            if (scrollView != null)
            {
                scrollView.ExtentChanged -= ScrollView_ExtentChanged;
                scrollView.StateChanged -= ScrollView_StateChanged;
                scrollView.ViewChanged -= ScrollView_ViewChanged;
                scrollView.ScrollAnimationStarting -= ScrollView_ScrollAnimationStarting;
                scrollView.ZoomAnimationStarting -= ScrollView_ZoomAnimationStarting;
            }
        }

        private void ChkLogScrollPresenterMessages_Checked(object sender, RoutedEventArgs e)
        {
            MUXControlsTestHooks.SetLoggingLevelForType("ScrollPresenter", isLoggingInfoLevel: true, isLoggingVerboseLevel: true);
            if (chkLogScrollViewMessages.IsChecked == false)
                MUXControlsTestHooks.LoggingMessage += MUXControlsTestHooks_LoggingMessage;
        }

        private void ChkLogScrollPresenterMessages_Unchecked(object sender, RoutedEventArgs e)
        {
            MUXControlsTestHooks.SetLoggingLevelForType("ScrollPresenter", isLoggingInfoLevel: false, isLoggingVerboseLevel: false);
            if (chkLogScrollViewMessages.IsChecked == false)
                MUXControlsTestHooks.LoggingMessage -= MUXControlsTestHooks_LoggingMessage;
        }

        private void ChkLogScrollViewMessages_Checked(object sender, RoutedEventArgs e)
        {
            MUXControlsTestHooks.SetLoggingLevelForType("ScrollView", isLoggingInfoLevel: true, isLoggingVerboseLevel: true);
            if (chkLogScrollPresenterMessages.IsChecked == false)
                MUXControlsTestHooks.LoggingMessage += MUXControlsTestHooks_LoggingMessage;
        }

        private void ChkLogScrollViewMessages_Unchecked(object sender, RoutedEventArgs e)
        {
            MUXControlsTestHooks.SetLoggingLevelForType("ScrollView", isLoggingInfoLevel: false, isLoggingVerboseLevel: false);
            if (chkLogScrollPresenterMessages.IsChecked == false)
                MUXControlsTestHooks.LoggingMessage -= MUXControlsTestHooks_LoggingMessage;
        }

        private void ChkAutoHideScrollControllers_Indeterminate(object sender, RoutedEventArgs e)
        {
            ScrollViewTestHooks.SetAutoHideScrollControllers(scrollView, null);
        }

        private void ChkAutoHideScrollControllers_Checked(object sender, RoutedEventArgs e)
        {
            ScrollViewTestHooks.SetAutoHideScrollControllers(scrollView, true);
        }

        private void ChkAutoHideScrollControllers_Unchecked(object sender, RoutedEventArgs e)
        {
            ScrollViewTestHooks.SetAutoHideScrollControllers(scrollView, false);
        }

        private void MUXControlsTestHooks_LoggingMessage(object sender, MUXControlsTestHooksLoggingMessageEventArgs args)
        {
            // Cut off the terminating new line.
            string msg = args.Message.Substring(0, args.Message.Length - 1);
            string asyncEventMessage = string.Empty;
            string senderName = string.Empty;

            try
            {
                FrameworkElement fe = sender as FrameworkElement;

                if (fe != null)
                {
                    senderName = "s:" + fe.Name + ", ";
                }
            }
            catch
            {
            }

            if (args.IsVerboseLevel)
            {
                asyncEventMessage = "Verbose: " + senderName + "m:" + msg;
            }
            else
            {
                asyncEventMessage = "Info: " + senderName + "m:" + msg;
            }

            AppendAsyncEventMessage(asyncEventMessage);
        }

        private void AppendAsyncEventMessage(string asyncEventMessage)
        {
            lock (asyncEventReportingLock)
            {
                while (asyncEventMessage.Length > 0)
                {
                    string msgHead = asyncEventMessage;

                    if (asyncEventMessage.Length > 110)
                    {
                        int commaIndex = asyncEventMessage.IndexOf(',', 110);
                        if (commaIndex != -1)
                        {
                            msgHead = asyncEventMessage.Substring(0, commaIndex);
                            asyncEventMessage = asyncEventMessage.Substring(commaIndex + 1);
                        }
                        else
                        {
                            asyncEventMessage = string.Empty;
                        }
                    }
                    else
                    {
                        asyncEventMessage = string.Empty;
                    }

                    lstAsyncEventMessage.Add(msgHead);
                }

                var ignored = this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, AppendAsyncEventMessage);
            }
        }

        private void AppendAsyncEventMessage()
        {
            lock (asyncEventReportingLock)
            {
                foreach (string asyncEventMessage in lstAsyncEventMessage)
                {
                    lstLogs.Items.Add(asyncEventMessage);
                }
                lstAsyncEventMessage.Clear();
            }
        }

        private void BtnClearExceptionReport_Click(object sender, RoutedEventArgs e)
        {
            txtExceptionReport.Text = string.Empty;
        }
    }
}

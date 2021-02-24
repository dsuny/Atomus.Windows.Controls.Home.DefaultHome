using Atomus.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atomus.Windows.Controls.Home
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DefaultHome : UserControl, IAction
    {
        private AtomusControlEventHandler beforeActionEventHandler;
        private AtomusControlEventHandler afterActionEventHandler;

        private IAction userControl;


        #region Init
        public DefaultHome()
        {
            InitializeComponent();

            this.AddControl();
        }
        #endregion

        #region Dictionary
        #endregion

        #region Spread
        #endregion

        #region IO
        object IAction.ControlAction(ICore sender, AtomusControlArgs e)
        {
            try
            {
                this.beforeActionEventHandler?.Invoke(this, e);

                switch (e.Action)
                {
                    default:
                        userControl.ControlAction(sender, e);
                        //throw new AtomusException("'{0}'은 처리할 수 없는 Action 입니다.".Translate(e.Action));

                        break;
                }

                return null;
            }
            finally
            {
                if (!new string[] { "Join.ClearPassword" }.Contains(e.Action))
                    this.afterActionEventHandler?.Invoke(this, e);
            }
        }
        #endregion

        #region Event
        event AtomusControlEventHandler IAction.BeforeActionEventHandler
        {
            add
            {
                this.beforeActionEventHandler += value;
            }
            remove
            {
                this.beforeActionEventHandler -= value;
            }
        }
        event AtomusControlEventHandler IAction.AfterActionEventHandler
        {
            add
            {
                this.afterActionEventHandler += value;
            }
            remove
            {
                this.afterActionEventHandler -= value;
            }
        }

        private void DefaultHome_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region ETC
        private void AddControl()
        {
            UserControl userControl;
            string[] temps;

            try
            {
                userControl = null;

                if (this.userControl == null)
                {
                    temps = this.GetAttribute("Controls").Split(',');

                    if (temps == null || temps.Count() < 1)
                        return;

                    foreach (string tmp in temps)
                    {
                        //this.userControl = new Atomus.Windows.Controls.WebBrowser.DefaultWebBrowser();
                        this.userControl = (IAction)this.CreateInstance(string.Format("Controls.{0}.Namespace", tmp));

                        this.userControl.AfterActionEventHandler += UserControl_AfterActionEventHandler;

                        userControl = (UserControl)this.userControl;

                        userControl.VerticalAlignment = VerticalAlignment.Stretch;
                        userControl.HorizontalAlignment = HorizontalAlignment.Stretch;

                        //if (this.GetAttribute(string.Format("Controls.{0}.Namespace", tmp)).Equals("Menu.GetControl"))
                        //{
                        //    AtomusControlEventArgs e1;

                        //    e1 = new AtomusControlEventArgs("Menu.GetControl", new object[] { this.GetAttributeDecimal(string.Format("Controls.{0}.MenuID", tmp))
                        //                                                        , this.GetAttributeDecimal(string.Format("Controls.{0}.AssemblyID", tmp))
                        //                                                        , null, false });

                        //    this.afterActionEventHandler?.Invoke(this, e1);

                        //    if (e1.Value is System.Windows.Forms.Control)
                        //        control = (System.Windows.Forms.Control)e1.Value;

                        //}
                        //else
                        //{
                        //    control = (System.Windows.Forms.Control)this.CreateInstance(string.Format("Controls.{0}.Namespace", tmp));

                        //    control.Dock = DockStyle.Fill;
                        //}

                        //if (control != null)
                        //{
                        //    this.SetDock(control, this.GetAttribute(string.Format("Controls.{0}.Dock", tmp)));

                        //    this.Controls.Add(control);

                        //    control.BringToFront();
                        //}
                    }



                }
                else
                    userControl = (UserControl)this.userControl;

                if (userControl == null)
                    return;

                userControl.Visibility = Visibility.Visible;

                if (!this.gridLayout.Children.Contains(userControl))
                    this.gridLayout.Children.Add(userControl);
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
                //(this).WindowsMessageBoxShow(Application.Current.Windows[0], ex);
            }
        }

        private void UserControl_AfterActionEventHandler(ICore sender, AtomusControlEventArgs e)
        {
        }
        #endregion

    }
}

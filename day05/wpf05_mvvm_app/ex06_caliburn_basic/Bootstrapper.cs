﻿using Caliburn.Micro;
using ex06_caliburn_basic.ViewModels;
using System.Windows;

namespace ex06_caliburn_basic
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper() 
        {
            Initialize(); // Caliburn.Micro MVVM 시작하도록 초기화
        }

        // MVVM 애플리케이션이 처음 시작될 때 이벤트핸들러
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            DisplayRootViewForAsync<MainViewModel>(); // MainViewModel과 뷰 화면을 합쳐서 표시
        }
    }
}

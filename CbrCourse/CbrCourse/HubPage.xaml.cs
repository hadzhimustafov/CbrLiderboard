﻿using ApiModule;
using Autofac;
using CbrCourse.Common;
using CbrCourse.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы концентратора задокументирован по адресу http://go.microsoft.com/fwlink/?LinkID=321224

namespace CbrCourse
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private MainViewModel defaultViewModel;

        /// <summary>
        /// NavigationHelper используется на каждой странице для облегчения навигации и 
        /// управление жизненным циклом процесса
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Эту настройку можно изменить на модель строго типизированных представлений.
        /// </summary>
        public MainViewModel DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new global::CbrModule.CbrModule());
            builder.RegisterModule(new global::ApiModule.ApiModule());
            builder.RegisterModule(new CoreModule());
            var cont = builder.Build();

            this.defaultViewModel = cont.Resolve<MainViewModel>();
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации.  Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="sender">
        /// Источник события; как правило, <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Данные события, предоставляющие параметр навигации, который передается
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы и
        /// словарь состояний, сохраненных этой страницей в ходе предыдущего
        /// сеанса.  Это состояние будет равно NULL при первом посещении страницы.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            //var sampleDataGroups = await SampleDataSource.GetGroupAsync().ConfigureAwait(false);
            this.DefaultViewModel.UpdateGroupAsync();
        }
        
        /// <summary>
        /// Вызывается при нажатии элемента внутри раздела.
        /// </summary>
        /// <param name="sender">Представление сетки или списка
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            var itemId = ((Valute)e.ClickedItem).ID;
            this.defaultViewModel.UpdateSelectedItem(itemId);
            this.Frame.Navigate(typeof(ItemPage), this.defaultViewModel);
        }
        #region Регистрация NavigationHelper

        /// Методы, предоставленные в этом разделе, используются исключительно для того, чтобы
        /// NavigationHelper для отклика на методы навигации страницы.
        /// 
        /// Логика страницы должна быть размещена в обработчиках событий для 
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// и <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// Параметр навигации доступен в методе LoadState 
        /// в дополнение к состоянию страницы, сохраненному в ходе предыдущего сеанса.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}

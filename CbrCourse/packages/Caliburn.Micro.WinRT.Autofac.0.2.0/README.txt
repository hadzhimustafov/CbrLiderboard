==INSTALL STEPS==

- Inherit your applicition class (MARKUP) from CaliburnAutofacApplication

<CaliburnAutofac:CaliburnAutofacApplication
    x:Class="Caliburn.Micro.WinRT.Autofac.Sample.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" 
    xmlns:CaliburnAutofac="using:Caliburn.Micro.WinRT.Autofac">

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Common/StandardStyles.xaml"/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</CaliburnAutofac:CaliburnAutofacApplication>

- Inherit your applicition class (CODE BEHIND) from CaliburnAutofacApplication. 

    sealed partial class App : CaliburnAutofacApplication
    {
        public App()
        {
            InitializeComponent();
        }
        
        /*Set root view*/
        public override void HandleOnLaunched()
        {
            DisplayRootView<MainPage>();
        }
        
        /*Register your classes to Autofac container.*/
        /*IMPORTANT: İf not override this method all types will register to Autopfac container by default.*/
        public override void HandleConfigure(ContainerBuilder builder)
        {
            /*Sample code register all types to Autofac container.*/
            builder.RegisterAssemblyTypes(AssemblySource.Instance.ToArray())
            .AsSelf()
            .InstancePerDependency();
        }
    }



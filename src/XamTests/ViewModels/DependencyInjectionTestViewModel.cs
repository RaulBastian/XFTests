using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamTests.ViewModels
{
    public class DependencyInjectionTestViewModel:BindableBase
    {
        private readonly IService1 service1;
        private readonly IService2 service2;
        private readonly int aa;

        public DependencyInjectionTestViewModel(IService1 service1, IService2 service2)
        {
            this.service1 = service1;
            this.service2 = service2;
        }

        public DependencyInjectionTestViewModel(IService1 service1)
        {
            this.service1 = service1;
        }

        public DependencyInjectionTestViewModel(int aa)
        {
            this.aa = aa;
        }
    }

    public interface IService1
    {
        void Method1();
    }

    public interface IService2
    {
        void Method2();
    }

    public class RepositoryService1 : IService1
    {
        public void Method1()
        {
            
        }
    }

    public class RepositoryService2 : IService2
    {
        public void Method2()
        {

        }
    }
}

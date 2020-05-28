using System;
using AutocorApi.Dependencias.Ninject;
using AutocorApi.Repositorios.Dapper;
using AutocorApi.Servicios.Core.Implementation;
using AutocorApi.Servicios.Dto;
using AutocorApi.Servicios.Email.Implementation;
using AutocorApi.Servicios.Mappings;
using Ninject;

namespace AutocorApi.Test.ConsoleTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // MappingConfig.Initialize();

            var kernel = new StandardKernel();
            kernel.Load(new ModuloServicios(), new ModuloRepositorios());

            

            Console.WriteLine("Fin de ejecución");
            Console.ReadKey();
        }
    }
}
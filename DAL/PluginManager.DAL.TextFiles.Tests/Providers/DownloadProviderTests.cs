﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: DownloadProviderTests.cs
 *
 *  Purpose:  Download provider test for text based storage
 *
 *  Date        Name                Reason
 *  25/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Downloads;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DownloadProviderTests : BaseProviderTests
    {
        [TestMethod]
        public void DownloadCategoriesGet_AnyUser_ReturnsCategoryListForAllUsers()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                        new DownloadCategoryDataRow() { Name = "Cat 2" },
                        new DownloadCategoryDataRow() { Name = "Cat 3" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                        new DownloadItemsDataRow() { Name = "DL3", UserId = 0, CategoryId = 1, Description = "dl3", Filename = "dl3.txt" },
                        new DownloadItemsDataRow() { Name = "DL4", UserId = 0, CategoryId = 1, Description = "dl4", Filename = "dl4.txt" },
                        new DownloadItemsDataRow() { Name = "DL5", UserId = 0, CategoryId = 1, Description = "dl5", Filename = "dl5.txt" },
                        new DownloadItemsDataRow() { Name = "DL6", UserId = 0, CategoryId = 2, Description = "dl6", Filename = "dl6.txt" },
                        new DownloadItemsDataRow() { Name = "DL7", UserId = 1, CategoryId = 2, Description = "dl7", Filename = "dl7.txt" },
                    });

                    List<DownloadCategory> result = sut.DownloadCategoriesGet(0);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);
                    Assert.AreEqual("Cat 1", result[0].Name);
                    Assert.AreEqual(1, result[0].Downloads.Count);

                    Assert.AreEqual("Cat 2", result[1].Name);
                    Assert.AreEqual(3, result[1].Downloads.Count);

                    Assert.AreEqual("Cat 3", result[2].Name);
                    Assert.AreEqual(1, result[2].Downloads.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void DownloadCategoriesGet_SpecificUser_ReturnsCategoryListForUser()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                        new DownloadCategoryDataRow() { Name = "Cat 2" },
                        new DownloadCategoryDataRow() { Name = "Cat 3" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                        new DownloadItemsDataRow() { Name = "DL3", UserId = 0, CategoryId = 1, Description = "dl3", Filename = "dl3.txt" },
                        new DownloadItemsDataRow() { Name = "DL4", UserId = 0, CategoryId = 1, Description = "dl4", Filename = "dl4.txt" },
                        new DownloadItemsDataRow() { Name = "DL5", UserId = 0, CategoryId = 1, Description = "dl5", Filename = "dl5.txt" },
                        new DownloadItemsDataRow() { Name = "DL6", UserId = 0, CategoryId = 2, Description = "dl6", Filename = "dl6.txt" },
                        new DownloadItemsDataRow() { Name = "DL7", UserId = 1, CategoryId = 2, Description = "dl7", Filename = "dl7.txt" },
                    });

                    List<DownloadCategory> result = sut.DownloadCategoriesGet(1);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);
                    Assert.AreEqual("Cat 1", result[0].Name);
                    Assert.AreEqual(2, result[0].Downloads.Count);

                    Assert.AreEqual("Cat 2", result[1].Name);
                    Assert.AreEqual(3, result[1].Downloads.Count);

                    Assert.AreEqual("Cat 3", result[2].Name);
                    Assert.AreEqual(2, result[2].Downloads.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void DownloadCategoriesGet_ReturnsCategoryListForAnyUser()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                        new DownloadCategoryDataRow() { Name = "Cat 2" },
                        new DownloadCategoryDataRow() { Name = "Cat 3" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                        new DownloadItemsDataRow() { Name = "DL3", UserId = 0, CategoryId = 1, Description = "dl3", Filename = "dl3.txt" },
                        new DownloadItemsDataRow() { Name = "DL4", UserId = 0, CategoryId = 1, Description = "dl4", Filename = "dl4.txt" },
                        new DownloadItemsDataRow() { Name = "DL5", UserId = 0, CategoryId = 1, Description = "dl5", Filename = "dl5.txt" },
                        new DownloadItemsDataRow() { Name = "DL6", UserId = 0, CategoryId = 2, Description = "dl6", Filename = "dl6.txt" },
                        new DownloadItemsDataRow() { Name = "DL7", UserId = 1, CategoryId = 2, Description = "dl7", Filename = "dl7.txt" },
                    });

                    List<DownloadCategory> result = sut.DownloadCategoriesGet();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);
                    Assert.AreEqual("Cat 1", result[0].Name);
                    Assert.AreEqual(1, result[0].Downloads.Count);

                    Assert.AreEqual("Cat 2", result[1].Name);
                    Assert.AreEqual(3, result[1].Downloads.Count);

                    Assert.AreEqual("Cat 3", result[2].Name);
                    Assert.AreEqual(1, result[2].Downloads.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetDownloadItem_ItemNotFound_ReturnsNull()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    DownloadItem result = sut.GetDownloadItem(38);
                    Assert.IsNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetDownloadItem_AllUsers_ReturnsDownLoadItem()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                    });

                    DownloadItem result = sut.GetDownloadItem(1);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("dl1", result.Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetDownloadItem_UsersSpecificFile_ReturnsDownLoadItem()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                    });

                    DownloadItem result = sut.GetDownloadItem(1, 1);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("dl1", result.Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ItemDownloaded_AllUsers_DownloadCountIncremented()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                    });

                    DownloadItem result = sut.GetDownloadItem(1);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("dl1", result.Description);

                    for (int i = 1; i < 100; i++)
                    {
                        sut.ItemDownloaded(result.Id);

                        DownloadItemsDataRow item = downloadItemsTable.Select(result.Id);
                        Assert.AreEqual(i, item.DownloadCount);
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ItemDownloaded_SpecificUser_DownloadCountIncremented()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                ISettingsProvider settingsProvider = new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\")));

                List<object> servicesList = new List<object>()
                {
                    new DownloadItemsDataRowDefaults(),
                    new UserDataRowDefaults(settingsProvider)
                };

                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<ISettingsProvider>(settingsProvider);
                services.AddSingleton<IMemoryCache>(new MockMemoryCache());
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
                    initialisation.AfterConfigure(mockApplicationBuilder);

                    DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
                    Assert.IsNotNull(sut);

                    ITextTableOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ITextTableOperations<DownloadCategoryDataRow>>();
                    Assert.IsNotNull(downloadCategoriesTable);

                    downloadCategoriesTable.Insert(new List<DownloadCategoryDataRow>()
                    {
                        new DownloadCategoryDataRow() { Name = "Cat 1" },
                    });

                    ITextTableOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ITextTableOperations<DownloadItemsDataRow>>();
                    Assert.IsNotNull(downloadItemsTable);

                    downloadItemsTable.Insert(new List<DownloadItemsDataRow>()
                    {
                        new DownloadItemsDataRow() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
                        new DownloadItemsDataRow() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
                    });

                    DownloadItem result = sut.GetDownloadItem(1, 1);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("dl1", result.Description);

                    for (int i = 1; i < 100; i++)
                    {
                        sut.ItemDownloaded(1, result.Id);

                        DownloadItemsDataRow item = downloadItemsTable.Select(result.Id);
                        Assert.AreEqual(i, item.DownloadCount);
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}

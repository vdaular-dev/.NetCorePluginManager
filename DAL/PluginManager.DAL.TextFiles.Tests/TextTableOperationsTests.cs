/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: TextTableOperationsTests.cs
 *
 *  Purpose:  TextTableOperationsTests tests for text based storage
 *
 *  Date        Name                Reason
 *  30/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.DAL.TextFiles.Internal;

using AspNetCore.PluginManager.Tests.Shared;

using io = System.IO;
using PluginManager.DAL.TextFiles.Tests.Mocks;
using System.IO;

#pragma warning disable CA1806

namespace PluginManager.DAL.TextFiles.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TextTableOperationsTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, 
                    new ForeignKeyManager(), new MockPluginClassesService());
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialize_InvalidParam_InitializerNull_Throws_ArgumentNullException()
        {
            using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(null, new ForeignKeyManager(), new MockPluginClassesService());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Initialize_InvalidParam_ForeignKeyManagerNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);
                new TextTableOperations<MockRow>(initializer, null, new MockPluginClassesService());
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Initialize_TableCanNotBeDeletedOrWrittenWhilstOpen()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService()))
                {
                    Assert.IsTrue(io.File.Exists(io.Path.Combine(directory, "MockTable.dat")));

                    try
                    {
                        io.File.Delete(io.Path.Combine(directory, "MockTable.dat"));
                    }
                    catch (System.IO.IOException err)
                    {
                        Assert.AreEqual($"The process cannot access the file '{directory}\\MockTable.dat' because it is being used by another process.", err.Message);
                    }
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Initialize_TableFileNotFound_CreatesNewTableFile_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());

                Assert.IsTrue(io.File.Exists(io.Path.Combine(directory, "MockTable.dat")));
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_Multiple_InvalidParamNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());

                sut.Insert(records: null);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_SingleRecord_InvalidParamNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());

                sut.Insert(record: null!);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Insert_SingleRecord_ClassAlreadyDisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.Insert(new MockRow());
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Insert_SingleRecord_ForcedWrite_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();
                FileInfo fileInfo = null;

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    sut.Insert(new MockRow());
                    fileInfo = new FileInfo(Path.Combine(directory, "MockTable.dat"));
                    Assert.AreEqual(140, fileInfo.Length);
                    Assert.AreEqual(1, sut.RecordCount);
                }

                fileInfo = new FileInfo(Path.Combine(directory, "MockTable.dat"));
                Assert.AreEqual(140, fileInfo.Length);

                using TextTableOperations<MockRow> sutRead = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService());
                IReadOnlyList<MockRow> records = sutRead.Select();

                Assert.AreEqual(1, records.Count);
                Assert.AreEqual(0, records[0].Id);
                Assert.AreEqual(0, sutRead.PrimarySequence);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Insert_SingleRecord_LazyWrite_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();
                FileInfo fileInfo = null;

                using (TextTableOperations<MockLazyWriteRow> sut = new TextTableOperations<MockLazyWriteRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    sut.Insert(new MockLazyWriteRow("some data"));
                    fileInfo = new FileInfo(Path.Combine(directory, "MockLazyWriteTable.dat"));
                    Assert.AreEqual(45, fileInfo.Length);
                    Assert.AreEqual(1, sut.RecordCount);
                }

                fileInfo = new FileInfo(Path.Combine(directory, "MockLazyWriteTable.dat"));
                Assert.AreEqual(159, fileInfo.Length);

                using TextTableOperations<MockLazyWriteRow> sutRead = new TextTableOperations<MockLazyWriteRow>(initializer, keyManager, new MockPluginClassesService());
                IReadOnlyList<MockLazyWriteRow> records = sutRead.Select();

                Assert.AreEqual(1, records.Count);
                Assert.AreEqual(0, records[0].Id);
                Assert.AreEqual(0, sutRead.PrimarySequence);
                Assert.AreEqual("some data", records[0].Data);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Insert_SingleRecord_OtherRecordsExist_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    sut.Insert(new MockRow());
                    sut.Insert(new MockRow());
                }

                using TextTableOperations<MockRow> sutRead = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService());
                IReadOnlyList<MockRow> records = sutRead.Select();

                Assert.AreEqual(2, records.Count);
                Assert.AreEqual(0, records[0].Id);
                Assert.AreEqual(1, records[1].Id);
                Assert.AreEqual(1, sutRead.PrimarySequence);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Delete_SingleRecord_ObjectAlreadydisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.Delete(new MockRow());

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_SingleRecord_ArgNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Delete(record: null!);

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Delete_SingleRecord_RecordIsRemoved_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRow> testData = new List<MockRow>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockRow(i));

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockRow> deleteSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, deleteSut.RecordCount);
                    Assert.AreEqual(1475355, deleteSut.DataLength);

                    deleteSut.Delete(deleteSut.Select(1519));
                }

                using (TextTableOperations<MockRow> readSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15167, readSut.RecordCount);
                    Assert.AreEqual(1475258, readSut.DataLength);

                    Assert.IsNull(readSut.Select(1519));
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Delete_Multiple_ObjectAlreadydisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.Delete(new List<MockRow>() { new MockRow() });

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Multiple_ArgNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Delete(records: null!);

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Delete_Multiple_RecordIsRemoved_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRow> testData = new List<MockRow>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockRow(i));

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockRow> deleteSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, deleteSut.RecordCount);
                    Assert.AreEqual(1475355, deleteSut.DataLength);

                    deleteSut.Delete(new List<MockRow>()
                    {
                        deleteSut.Select(1519),
                        deleteSut.Select(2168),
                        deleteSut.Select(15000)
                    });
                }

                using (TextTableOperations<MockRow> readSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15165, readSut.RecordCount);
                    Assert.AreEqual(1475063, readSut.DataLength);

                    Assert.IsNull(readSut.Select(1519));
                    Assert.IsNull(readSut.Select(2168));
                    Assert.IsNull(readSut.Select(15000));
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CompactPercent_AfterMultipleRowsRemoved_IsAccurate_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRow> testData = new List<MockRow>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockRow(i));

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockRow> deleteSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, deleteSut.RecordCount);
                    Assert.AreEqual(1475355, deleteSut.DataLength);

                    List<MockRow> deleteList = new List<MockRow>();
                    IReadOnlyList<MockRow> current = deleteSut.Select();

                    for (int i = 10; i < 5000; i++)
                        deleteList.Add(current[i]);

                    deleteSut.Delete(deleteList);
                    Assert.AreEqual(67, deleteSut.CompactPercent);
                }

                using (TextTableOperations<MockRow> readSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(10178, readSut.RecordCount);
                    Assert.AreEqual(992405, readSut.DataLength);
                    Assert.AreEqual(67, readSut.CompactPercent);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Update_SingleRecord_ObjectAlreadydisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockUpdateRow> sut = new TextTableOperations<MockUpdateRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.Update(new MockUpdateRow());

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_SingleRecord_ArgNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockUpdateRow> sut = new TextTableOperations<MockUpdateRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Update(record: null!);

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Update_SingleRecord_RecordIsUpdated_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockUpdateRow> sut = new TextTableOperations<MockUpdateRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockUpdateRow> testData = new List<MockUpdateRow>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockUpdateRow(i));

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockUpdateRow> updateSut = new TextTableOperations<MockUpdateRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, updateSut.RecordCount);
                    Assert.AreEqual(1657371, updateSut.DataLength);

                    MockUpdateRow row1 = updateSut.Select(8192);
                    row1.Data = "not null data";
                    updateSut.Update(row1);
                }

                using (TextTableOperations<MockUpdateRow> readSut = new TextTableOperations<MockUpdateRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, readSut.RecordCount);
                    Assert.AreEqual(1657382, readSut.DataLength);

                    Assert.IsNotNull(readSut.Select(8192));
                    Assert.AreEqual("not null data", readSut.Select(8192).Data);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Update_Multiple_ObjectAlreadydisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockUpdateRow> sut = new TextTableOperations<MockUpdateRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.Update(new List<MockUpdateRow>() { new MockUpdateRow() });

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Multiple_ArgNull_Throws_ArgumentNullException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockUpdateRow> sut = new TextTableOperations<MockUpdateRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Update(records: null!);

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Update_Multiple_RecordIsUpdated_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockUpdateRow> sut = new TextTableOperations<MockUpdateRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockUpdateRow> testData = new List<MockUpdateRow>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockUpdateRow(i));

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockUpdateRow> updateSut = new TextTableOperations<MockUpdateRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, updateSut.RecordCount);
                    Assert.AreEqual(1657371, updateSut.DataLength);

                    List<MockUpdateRow> updateList = new List<MockUpdateRow>()
                    {
                        updateSut.Select(1519),
                        updateSut.Select(2168),
                        updateSut.Select(15000)
                    };

                    updateList[0].Data = "Row 1 updated";
                    updateList[1].Data = "Row 2 updated";
                    updateList[2].Data = "Row 3 updated";
                    updateSut.Update(updateList);
                }

                using (TextTableOperations<MockUpdateRow> readSut = new TextTableOperations<MockUpdateRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, readSut.RecordCount);
                    Assert.AreEqual(1657404, readSut.DataLength);

                    Assert.IsNotNull(readSut.Select(1519));
                    Assert.AreEqual("Row 1 updated", readSut.Select(1519).Data);
                    Assert.IsNotNull(readSut.Select(2168));
                    Assert.AreEqual("Row 2 updated", readSut.Select(2168).Data);
                    Assert.IsNotNull(readSut.Select(15000));
                    Assert.AreEqual("Row 3 updated", readSut.Select(15000).Data);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Insert_Multiple_ObjectAlreadyDisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.Insert(new List<MockRow>());

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Insert_Multiple_Writes15168Records_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRow> testData = new List<MockRow>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockRow(i));

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockRow> readSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, readSut.RecordCount);
                    Assert.AreEqual(1475355, readSut.DataLength);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Insert_Multiple_Writes5RecordsWithCorrectSequence_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRow> testData = new List<MockRow>();

                    for (int i = 0; i < 5; i++)
                        testData.Add(new MockRow());

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockRow> readSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(5, readSut.RecordCount);
                    Assert.AreEqual(471, readSut.DataLength);
                    Assert.AreEqual(4L, readSut.PrimarySequence);

                    IReadOnlyList<MockRow> records = readSut.Select();

                    for (int i = 0; i < readSut.RecordCount; i++)
                        Assert.AreEqual(i, records[i].Id);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Insert_Multiple_Writes15168Records_Compressed_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRowCompressed> sut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRowCompressed> testData = new List<MockRowCompressed>();

                    for (int i = 0; i < 15168; i++)
                        testData.Add(new MockRowCompressed());

                    sut.Insert(testData);
                }

                using (TextTableOperations<MockRowCompressed> readSut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(15168, readSut.RecordCount);
                    IReadOnlyList<MockRowCompressed> testData = readSut.Select();
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Select_ById_ObjectDisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                _ = sut.Select(1);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Select_ById_InstanceNotFound_Returns_Null()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                MockRow row = sut.Select(1);

                Assert.IsNull(row);
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Select_ById_RecordFoundAndReturned_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRowCompressed> sut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRowCompressed> testData = new List<MockRowCompressed>();

                    for (int i = 0; i < 200; i++)
                        testData.Add(new MockRowCompressed());

                    sut.Insert(testData);

                    MockRowCompressed row = sut.Select(101);
                    Assert.IsNotNull(row);
                    Assert.AreEqual(101, row.Id);
                }


                using (TextTableOperations<MockRowCompressed> readSut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(200, readSut.RecordCount);
                    IReadOnlyList<MockRowCompressed> testData = readSut.Select();

                    MockRowCompressed row = readSut.Select(101);
                    Assert.IsNotNull(row);
                    Assert.AreEqual(101, row.Id);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void NextSequence_ObjectAlreadyDisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                _ = sut.NextSequence();

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void NextSequence_IncrementsAndIsSavedStraightAwayToFile_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);

                for (int i = 0; i < 100; i++)
                {
                    using (TextTableOperations<MockRowCompressed> sut = new TextTableOperations<MockRowCompressed>(initializer, new ForeignKeyManager(), new MockPluginClassesService()))
                    {
                        long nextSequence = sut.NextSequence();
                        Assert.AreEqual(i, (long)nextSequence);
                        Assert.AreEqual(i, sut.PrimarySequence);
                    }
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void ResetSequence_ObjectAlreadyDisposed_Throws_ObjectDisposedException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                TextTableInitializer initializer = CreateTestInitializer(directory);

                using TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, new ForeignKeyManager(), new MockPluginClassesService());
                sut.Dispose();
                sut.ResetSequence(123, 1);

            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ResetSequence_SetsCorrectValue_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRowCompressed> sut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    sut.ResetSequence(368745, -3287);
                }

                using (TextTableOperations<MockRowCompressed> sut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    long nextPrimarySequence = sut.NextSequence();
                    Assert.AreEqual(368746L, nextPrimarySequence);
                    Assert.AreEqual(368746L, sut.PrimarySequence);

                    long nextSecondarySequence = sut.NextSecondarySequence(1);
                    Assert.AreEqual(-3286L, sut.SecondarySequence);
                    Assert.AreEqual(-3286L, sut.SecondarySequence);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ImmutableId_AfterLoadedFromDisk_Throws_InvalidOperationException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRowCompressed> sut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockRowCompressed> testData = new List<MockRowCompressed>();

                    MockRowCompressed row = new MockRowCompressed();
                    testData.Add(row);

                    row.Id = 10;

                    sut.Insert(row);

                    Assert.AreEqual(0, row.Id);

                    row = sut.Select(0);
                    Assert.IsNotNull(row);
                    Assert.AreEqual(0, row.Id);

                    bool exraised = false;
                    try
                    {
                        row.Id = -87;
                    }
                    catch (InvalidOperationException)
                    {
                        exraised = true;
                    }

                    Assert.IsTrue(exraised);
                }


                using (TextTableOperations<MockRowCompressed> readSut = new TextTableOperations<MockRowCompressed>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(1, readSut.RecordCount);
                    IReadOnlyList<MockRowCompressed> testData = readSut.Select();
                    Assert.AreEqual(1, testData.Count);
                    Assert.AreEqual(0, testData[0].Id);

                    MockRowCompressed row = readSut.Select(0);
                    Assert.IsNotNull(row);
                    Assert.AreEqual(0, row.Id);

                    row.Id = 87364;
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void InsertOrUpdate_InsertMultipleNewRows_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRow> sut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    for (int i = 0; i < 5; i++)
                        sut.InsertOrUpdate(new MockRow() { Id = -1});
                }

                using (TextTableOperations<MockRow> readSut = new TextTableOperations<MockRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(5, readSut.RecordCount);
                    Assert.AreEqual(471, readSut.DataLength);
                    Assert.AreEqual(4L, readSut.PrimarySequence);

                    IReadOnlyList<MockRow> records = readSut.Select();

                    for (int i = 0; i < readSut.RecordCount; i++)
                        Assert.AreEqual(i, records[i].Id);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void InsertOrUpdate_UpdateMultipleNewRows_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockTableUserRow> mockUsers = new TextTableOperations<MockTableUserRow>(initializer, keyManager, new MockPluginClassesService()))
                {
                    List<MockTableUserRow> testData = new List<MockTableUserRow>();

                    for (int i = 0; i < 5; i++)
                        testData.Add(new MockTableUserRow(i));

                    mockUsers.Insert(testData);

                    using (TextTableOperations<MockTableAddressRow> sut = new TextTableOperations<MockTableAddressRow>(initializer, keyManager, new MockPluginClassesService()))
                    {
                        List<MockTableAddressRow> itemsToInsert = new List<MockTableAddressRow>();

                        for (int i = 0; i < 5; i++)
                            itemsToInsert.Add(new MockTableAddressRow() { Id = -1, Description = "test", UserId = 1 });

                        sut.Insert(itemsToInsert);
                    }

                    using (TextTableOperations<MockTableAddressRow> readSut = new TextTableOperations<MockTableAddressRow>(initializer, keyManager, new MockPluginClassesService()))
                    {
                        Assert.AreEqual(5, readSut.RecordCount);
                        Assert.AreEqual(631, readSut.DataLength);
                        Assert.AreEqual(4L, readSut.PrimarySequence);

                        IReadOnlyList<MockTableAddressRow> records = readSut.Select();

                        for (int i = 0; i < readSut.RecordCount; i++)
                        {
                            Assert.AreEqual(i, records[i].Id);
                            Assert.AreEqual("test", records[i].Description);
                        }

                        records[2].Description = "updated";
                        readSut.InsertOrUpdate(records[2]);
                        readSut.Delete(records[4]);
                    }

                    using (TextTableOperations<MockTableAddressRow> readSut = new TextTableOperations<MockTableAddressRow>(initializer, keyManager, new MockPluginClassesService()))
                    {
                        Assert.AreEqual(4, readSut.RecordCount);
                        Assert.AreEqual(508, readSut.DataLength);
                        Assert.AreEqual(4L, readSut.PrimarySequence);

                        IReadOnlyList<MockTableAddressRow> records = readSut.Select();

                        Assert.AreEqual("updated", records[2].Description);
                    }
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        private static TextTableInitializer CreateTestInitializer(string path)
        {
            return new TextTableInitializer(path);
        }

        [TestMethod]
        public void Insert_IndexSpansMultipleProperties_Success()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRowMultipleIndex> sut = new TextTableOperations<MockRowMultipleIndex>(initializer, keyManager, new MockPluginClassesService()))
                {
                    for (int i = 0; i < 5; i++)
                        sut.InsertOrUpdate(new MockRowMultipleIndex() { Name = $"Name {i}", Index = i });
                }

                using (TextTableOperations<MockRowMultipleIndex> readSut = new TextTableOperations<MockRowMultipleIndex>(initializer, keyManager, new MockPluginClassesService()))
                {
                    Assert.AreEqual(5, readSut.RecordCount);
                    Assert.AreEqual(601, readSut.DataLength);
                    Assert.AreEqual(4L, readSut.PrimarySequence);

                    IReadOnlyList<MockRowMultipleIndex> records = readSut.Select();

                    for (int i = 0; i < readSut.RecordCount; i++)
                        Assert.AreEqual(i, records[i].Id);
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(UniqueIndexException))]
        public void Insert_IndexSpansMultipleProperties_ValueAlreadyIndexed_ThrowsUniqueIndexException()
        {
            string directory = io.Path.Combine(io.Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                io.Directory.CreateDirectory(directory);
                ITextTableInitializer initializer = CreateTestInitializer(directory);
                IForeignKeyManager keyManager = new ForeignKeyManager();

                using (TextTableOperations<MockRowMultipleIndex> sut = new TextTableOperations<MockRowMultipleIndex>(initializer, keyManager, new MockPluginClassesService()))
                {
                    for (int i = 0; i < 5; i++)
                        sut.InsertOrUpdate(new MockRowMultipleIndex() { Name = $"Name {i}", Index = i });

                    sut.Insert(new MockRowMultipleIndex() { Name = "Name 4", Index = 4 });
                }
            }
            finally
            {
                io.Directory.Delete(directory, true);
            }
        }
    }
}

#pragma warning restore CA1806
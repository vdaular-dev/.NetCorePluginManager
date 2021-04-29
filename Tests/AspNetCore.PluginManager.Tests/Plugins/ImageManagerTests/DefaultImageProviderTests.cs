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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ImageManagerControllerTests.cs
 *
 *  Purpose:  Unit tests for Image Manager Controller
 *
 *  Date        Name                Reason
 *  16/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultImageProviderTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidSettingsProvider_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = new DefaultImageProvider(new TestHostEnvironment(), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidIHostEnvironment_Null_Throws_ArgumentNullException()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(null, testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_Success()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(new TestHostEnvironment(), testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceWithEmptyPath_Success()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(new TestHostEnvironment(), testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateGroup_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathCreated = sut.CreateGroup(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateGroup_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathCreated = sut.CreateGroup("");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void CreateGroup_GroupCreated_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "TestGroup");
            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            bool pathCreated = sut.CreateGroup("TestGroup");

            Assert.IsTrue(pathCreated);
            Assert.IsTrue(Directory.Exists(newGroupPath));

            Directory.Delete(newGroupPath);
            Directory.Delete(testPath);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void CreateGroup_DuplicateGroupName_ReturnsFalse()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "TestGroup");
            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            bool pathCreated = sut.CreateGroup("TestGroup");

            Assert.IsTrue(pathCreated);
            Assert.IsTrue(Directory.Exists(newGroupPath));

            pathCreated = sut.CreateGroup(newGroupPath);

            Assert.IsFalse(pathCreated);

            Directory.Delete(newGroupPath);
            Directory.Delete(testPath);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteGroup_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathDeleted = sut.DeleteGroup(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteGroup_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathDeleted = sut.DeleteGroup("");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteGroup_PathDoesNotExist_ReturnsFalse()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathDeleted = sut.DeleteGroup("Not Exists");

            Assert.IsFalse(pathDeleted);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteGroup_GroupContainsItems_ReturnsSuccess()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            Directory.CreateDirectory(Path.Combine(newGroupPath, "Second Group"));
            Directory.CreateDirectory(Path.Combine(newGroupPath, "Third Group"));
            File.WriteAllText(Path.Combine(newGroupPath, "Second Group", "test.txt"), "test file");

            bool pathDeleted = sut.DeleteGroup("FirstGroup");

            Assert.IsTrue(pathDeleted);

            Assert.IsFalse(Directory.Exists(newGroupPath));

            Directory.Delete(testPath, true);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Groups_Retrieve_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");
            sut.CreateGroup("SecondGroup");

            Directory.CreateDirectory(Path.Combine(newGroupPath, "First Sub Group"));
            Directory.CreateDirectory(Path.Combine(newGroupPath, "Second Sub Group"));
            File.WriteAllText(Path.Combine(newGroupPath, "Second Sub Group", "test.txt"), "test file");

            Dictionary<string, List<string>> groups = sut.Groups();

            Directory.Delete(testPath, true);

            Assert.IsNotNull(groups);

            Assert.AreEqual(2, groups.Count);
            Assert.IsTrue(groups.ContainsKey("FirstGroup"));
            Assert.IsTrue(groups.ContainsKey("SecondGroup"));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Images_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            sut.Images(null);
        }

        //[TestMethod]
        //[TestCategory(ImageManagerTestsCategory)]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void Images_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        //{
        //    DefaultImageProvider sut = CreateDefaultImageProvider();

        //    sut.Images("");
        //}

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentException))]
        public void Images_InvalidGroupName_NotFound_Throws_ArgumentException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("Fantasy Image Group");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidGroupName_ReturnsListOfImages_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            List<ImageFile> images = sut.Images("FirstGroup");

            Directory.Delete(testPath, true);

            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images.Where(i => i.Name.Equals("file1.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("file2.txt")).Any());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidateUri_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");

            List<ImageFile> images = sut.Images("FirstGroup");

            Directory.Delete(testPath, true);

            Assert.AreEqual(1, images.Count);

            ImageFile imageFile = images.Where(i => i.Name.Equals("file1.txt")).FirstOrDefault();

            Assert.IsNotNull(imageFile);
            Assert.AreEqual("/images/FirstGroup/file1.txt", imageFile.ImageUri.ToString());
            Assert.IsFalse(imageFile.ImageUri.IsAbsoluteUri);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidateUri_WithoutGroupName_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = testPath;

            if (!Directory.Exists(newGroupPath))
                Directory.CreateDirectory(newGroupPath);

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");

            List<ImageFile> images = sut.Images("");

            Directory.Delete(testPath, true);

            Assert.AreEqual(1, images.Count);

            ImageFile imageFile = images.Where(i => i.Name.Equals("file1.txt")).FirstOrDefault();

            Assert.IsNotNull(imageFile);
            Assert.AreEqual("/images/file1.txt", imageFile.ImageUri.ToString());
            Assert.IsFalse(imageFile.ImageUri.IsAbsoluteUri);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images(null, "valid string");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("", "valid string");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidSubgroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("valid string", "");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidSubgroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("valid string", null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentException))]
        public void SubgroupImages_InvalidSubgroupName_NotFound_Throws_ArgumentException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("valid string", "my subgroup");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Groups_ValidSubgroupName_ReturnsListOfImagesAndSubGroupNames_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");
            string subGroup1 = Path.Combine(newGroupPath, "SubGroup 1");
            string subGroup2 = Path.Combine(newGroupPath, "SubGroup 2");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            Assert.IsFalse(sut.GroupExists("FirstGroup"));

            bool addGroup = sut.CreateGroup("FirstGroup");
            Assert.IsTrue(addGroup);
            Assert.IsTrue(sut.GroupExists("FirstGroup"));

            sut.AddSubGroup("FirstGroup", "SubGroup 1");
            Assert.IsTrue(sut.SubGroupExists("FirstGroup", "SubGroup 1"));

            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file1.txt"), "text for file 1 in sub group 1");
            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file2.txt"), "text for file 2 in sub group 1");

            bool addSubGroup = sut.AddSubGroup("FirstGroup", "SubGroup 2");
            Assert.IsTrue(addSubGroup);
            Assert.IsTrue(sut.SubGroupExists("FirstGroup", "SubGroup 2"));
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file1.txt"), "text for file 1 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file2.txt"), "text for file 2 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file3.txt"), "text for file 3 in sub group 2");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            addSubGroup = sut.AddSubGroup("FirstGroup", "SubGroup 2");
            Assert.IsFalse(addSubGroup);

            List<ImageFile> images = sut.Images("FirstGroup", "Subgroup 2");

            Assert.AreEqual(3, images.Count);
            Assert.IsTrue(images.Where(i => i.Name.Equals("subgroup 2 file1.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("subgroup 2 file2.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("subgroup 2 file3.txt")).Any());

            Directory.Delete(testPath, true);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Groups_ValidGroupName_WithSubGroups_ReturnsListOfImagesAndSubGroupNames_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");
            string subGroup1 = Path.Combine(newGroupPath, "SubGroup 1");
            string subGroup2 = Path.Combine(newGroupPath, "SubGroup 2");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            Assert.IsFalse(sut.GroupExists("FirstGroup"));

            bool addGroup = sut.CreateGroup("FirstGroup");
            Assert.IsTrue(addGroup);
            Assert.IsTrue(sut.GroupExists("FirstGroup"));

            sut.AddSubGroup("FirstGroup", "SubGroup 1");
            Assert.IsTrue(sut.SubGroupExists("FirstGroup", "SubGroup 1"));

            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file1.txt"), "text for file 1 in sub group 1");
            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file2.txt"), "text for file 2 in sub group 1");

            bool addSubGroup = sut.AddSubGroup("FirstGroup", "SubGroup 2");
            Assert.IsTrue(addSubGroup);
            Assert.IsTrue(sut.SubGroupExists("FirstGroup", "SubGroup 2"));
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file1.txt"), "text for file 1 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file2.txt"), "text for file 2 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file3.txt"), "text for file 3 in sub group 2");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            addSubGroup = sut.AddSubGroup("FirstGroup", "SubGroup 2");
            Assert.IsFalse(addSubGroup);

            List<ImageFile> images = sut.Images("FirstGroup");

            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images.Where(i => i.Name.Equals("file1.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("file2.txt")).Any());

            Dictionary<string, List<string>> group = sut.Groups();

            Assert.AreEqual(1, group.Count);
            Assert.AreEqual(2, group["FirstGroup"].Count);
            Assert.AreEqual("SubGroup 1", group["FirstGroup"][0]);
            Assert.AreEqual("SubGroup 2", group["FirstGroup"][1]);

            bool subGroupDeleted = sut.DeleteSubGroup("FirstGroup", "SubGroup 1");
            Assert.IsTrue(subGroupDeleted);
            Assert.IsFalse(sut.SubGroupExists("FirstGroup", "SubGroup 1"));

            subGroupDeleted = sut.DeleteSubGroup("FirstGroup", "SubGroup 2");
            Assert.IsTrue(subGroupDeleted);
            Assert.IsFalse(sut.SubGroupExists("FirstGroup", "SubGroup 2"));

            subGroupDeleted = sut.DeleteSubGroup("FirstGroup", "SubGroup 2");
            Assert.IsFalse(subGroupDeleted);

            bool groupDeleted = sut.DeleteGroup("FirstGroup");
            Assert.IsTrue(groupDeleted);

            groupDeleted = sut.DeleteGroup("FirstGroup");
            Assert.IsFalse(groupDeleted);

            Directory.Delete(testPath, true);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupExists_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.GroupExists(null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupExists_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.GroupExists("");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubGroupExists(null, "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubGroupExists("", "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamSubroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubGroupExists("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamSubroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubGroupExists("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }







        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubGroup(null, "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubGroup("", "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamSubroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubGroup("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamSubroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubGroup("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }












        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubGroup(null, "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubGroup("", "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamSubroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubGroup("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamSubroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubGroup("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }






        private DefaultImageProvider CreateDefaultImageProvider(string imagePath = "")
        {
            if (imagePath == null)
                imagePath = String.Empty;

            if (!String.IsNullOrEmpty(imagePath))
                imagePath = imagePath.Replace("\\", "\\\\");

            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + imagePath + "\"}}");

            return new DefaultImageProvider(new TestHostEnvironment(), testSettingsProvider);
        }
    }
}

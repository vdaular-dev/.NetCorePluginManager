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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Image Manager Plugin
 *  
 *  File: ImagesViewModel.cs
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware.Images;

using SharedPluginFeatures;

namespace ImageManager.Plugin.Models
{
    /// <summary>
    /// View model used when viewing images using <see cref="Controllers.ImageManagerController"/>
    /// </summary>
    public sealed class ImagesViewModel : BaseModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ImagesViewModel()
        {

        }

        /// <summary>
        /// Constructor for use when displaying data within controller
        /// </summary>
        /// <param name="modelData"></param>
        /// <param name="selectedGroupName">Name of group, or empty string if root path</param>
        /// <param name="groups">List of all groups</param>
        /// <param name="imageFiles">List of images that belong to the group</param>
        public ImagesViewModel(in BaseModelData modelData, string selectedGroupName, List<string> groups, List<ImageFile> imageFiles)
            : base(modelData)
        {
            SelectedGroupName = selectedGroupName ?? throw new ArgumentNullException(nameof(selectedGroupName));
            Groups = groups ?? throw new ArgumentNullException(nameof(groups));
            ImageFiles = imageFiles ?? throw new ArgumentNullException(nameof(imageFiles));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of group
        /// </summary>
        /// <value>string</value>
        public string SelectedGroupName { get; set; }

        /// <summary>
        /// List of all image groups
        /// </summary>
        /// <value>List&lt;string&gt;</value>
        public List<string> Groups { get; set; }

        /// <summary>
        /// List of images that belong to the group
        /// </summary>
        /// <value>List&lt;ImageFile&gt;</value>
        public List<ImageFile> ImageFiles { get; }

        #endregion Properties
    }
}

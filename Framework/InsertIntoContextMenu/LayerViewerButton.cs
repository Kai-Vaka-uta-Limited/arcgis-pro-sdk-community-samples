﻿//Copyright 2015 Esri

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Internal.CIM;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;

namespace InsertIntoContextMenu
{
    internal class LayerViewerButton : Button
    {
        protected async override void OnClick()
        {
            string xml = "";
            //var toc = MapView.Active.MappingModule.ActiveTOC;
            if (MapView.Active != null)
            {
                // get toc highlighted layers
                var selLayers = MapView.Active.GetSelectedLayers();
                // retrieve the first one
                Layer layer = selLayers.FirstOrDefault();
                if (layer != null)
                {
                    // find the CIM and serialize it                    
                    await QueuedTask.Run(() =>
                        {
                            CIMBaseLayer cim = layer.GetDefinition();
                            xml = XmlUtil.SerializeCartoXObject(cim);
                        });                    
                }
            }

            if (string.IsNullOrEmpty(xml))
                return;

            // show it
            CIMViewerViewModel vm = new CIMViewerViewModel();            
            vm.Xml = xml;
            ArcGIS.Desktop.Internal.Framework.DialogManager.ShowDialog(vm, null);
        }
    }
}

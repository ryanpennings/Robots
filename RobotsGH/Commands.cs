using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Robots.Grasshopper.Commands
{
    [Obsolete]
    public class Custom : GH_Component
    {
        public Custom() : base(" command", "CustomCmd", "Custom command written in the manufacturer specific language", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
        public override bool Obsolete => true;
        public override Guid ComponentGuid => new Guid("{D15B1F9D-B3B9-4105-A365-234C1329B092}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconCustomCommand;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name", GH_ParamAccess.item, "Custom command");
            pManager.AddTextParameter("ABB decl", "Ad", "ABB variable declaration and assignment", GH_ParamAccess.item);
            pManager.AddTextParameter("KUKA decl", "Kd", "KUKA variable declaration and assignment", GH_ParamAccess.item);
            pManager.AddTextParameter("UR decl", "Ud", "UR variable declaration and assignment", GH_ParamAccess.item);
            pManager.AddTextParameter("ABB code", "A", "ABB code", GH_ParamAccess.item);
            pManager.AddTextParameter("KUKA code", "K", "KUKA code", GH_ParamAccess.item);
            pManager.AddTextParameter("UR code", "U", "UR code", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
            pManager[3].Optional = true;
            pManager[4].Optional = true;
            pManager[5].Optional = true;
            pManager[6].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null;
            string abbDeclaration = null, kukaDeclaration = null, urDeclaration = null;
            string abbCode = null, kukaCode = null, urCode = null;

            if (!DA.GetData(0, ref name)) { return; }
            DA.GetData(1, ref abbDeclaration);
            DA.GetData(2, ref kukaDeclaration);
            DA.GetData(3, ref urDeclaration);
            DA.GetData(4, ref abbCode);
            DA.GetData(5, ref kukaCode);
            DA.GetData(6, ref urCode);

            var command = new Robots.Commands.Custom(name);
            command.AddCommand(Manufacturers.ABB, abbCode, abbDeclaration);
            command.AddCommand(Manufacturers.KUKA, kukaCode, kukaDeclaration);
            command.AddCommand(Manufacturers.UR, urCode, urDeclaration);

            DA.SetData(0, new GH_Command(command));
        }
    }

    public class CustomCommand : GH_Component
    {
        public CustomCommand() : base("Custom command", "CustomCmd", "Custom command written in the manufacturer specific language", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("{713A6DF0-6C73-477F-8CA5-2FE18F3DE7C4}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconCustomCommand;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name", GH_ParamAccess.item, "Custom command");
            pManager.AddTextParameter("Manufacturer", "M", "Robot manufacturer, options:  ABB, KUKA, UR, FANUC, Staubli, Other, All. If you select 'All', the command will always be included irrespective of the manufacturer.", GH_ParamAccess.item, "All");
            pManager.AddTextParameter("Code", "C", "Command code", GH_ParamAccess.item);
            pManager.AddTextParameter("Declaration", "D", "Variable declaration and assignment", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null;
            string manufacturerText = null;
            string code = null, declaration = null;

            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref manufacturerText)) { return; }
            DA.GetData(2, ref code);
            DA.GetData(3, ref declaration);

            var command = new Robots.Commands.Custom(name);

            if (!Enum.TryParse<Manufacturers>(manufacturerText, out var manufacturer))
            {
                throw new ArgumentException($"Manufacturer {manufacturerText} not valid.");
            }

            command.AddCommand(manufacturer, code, declaration);
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class Group : GH_Component
    {
        public Group() : base("Group command", "GroupCmd", "Group of commands", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        public override Guid ComponentGuid => new Guid("{17485955-818B-4D0E-9986-26264E1F86DC}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconGroupCommand;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Commands", "C", "Group of commands", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var commands = new List<GH_Command>();

            if (!DA.GetDataList(0, commands)) { return; }

            var command = new Robots.Commands.Group();
            command.AddRange(commands.Select(x => x?.Value));
            DA.SetData(0, new GH_Command(command));
        }
    }


    public class SetDO : GH_Component
    {
        public SetDO() : base("Set DO", "SetDO", "Set digital output", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{C2F263E3-BF97-4E48-B2CB-42D3A5FE6190}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconSetDO;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DO", "D", "Digital output number", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Value", "V", "Digital output value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int DO = 0;
            bool value = false;

            if (!DA.GetData(0, ref DO)) { return; }
            if (!DA.GetData(1, ref value)) { return; }

            var command = new Robots.Commands.SetDO(DO, value);
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class SetAO : GH_Component
    {
        public SetAO() : base("Set AO", "SetAO", "Set analog output", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{CAA1A764-D588-4D63-95EA-9C8D43374B8D}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconSetAO;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("AO", "A", "Analog output number", GH_ParamAccess.item);
            pManager.AddNumberParameter("Value", "V", "Analog output value", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int AO = 0;
            double value = 0;

            if (!DA.GetData(0, ref AO)) { return; }
            if (!DA.GetData(1, ref value)) { return; }

            var command = new Robots.Commands.SetAO(AO, value);
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class PulseDO : GH_Component
    {
        public PulseDO() : base("Pulse DO", "PulseDO", "Send a pulse to a digital output", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{3CBDCD59-9621-4A0F-86BF-F4CC876E360D}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconPulseDO;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DO", "D", "Digital output number", GH_ParamAccess.item);
            pManager.AddNumberParameter("Time", "T", "Duration of pulse", GH_ParamAccess.item, 0.2);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int DO = 0;
            double length = 0;

            if (!DA.GetData(0, ref DO)) return;
            if (!DA.GetData(1, ref length)) return;

            var command = new Robots.Commands.PulseDO(DO, length);
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class Wait : GH_Component
    {
        public Wait() : base("Wait", "Wait", "Stops the program for a specific amount of time", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{5E7BA355-7EAC-4A5D-B736-286043AB0A45}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconWait;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Time", "T", "Time in seconds", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double time = 0;

            if (!DA.GetData(0, ref time)) return;

            var command = new Robots.Commands.Wait(time);
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class WaitDI : GH_Component
    {
        public WaitDI() : base("Wait DI", "WaitDI", "Stops the program until a digital input is turned on", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{8A930C8F-3BCE-4476-9E30-3F5C23DB2FB9}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconWaitDI;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("DI", "D", "Digital input number", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int DI = 0;

            if (!DA.GetData(0, ref DI)) { return; }

            var command = new Robots.Commands.WaitDI(DI);
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class Stop : GH_Component
    {
        public Stop() : base("Stop program", "Stop", "Stops the program until an operator starts it again", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{80E4E1AD-D1C0-441F-BDC5-5E810BCECE61}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconStopCommand;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var command = new Robots.Commands.Stop();
            DA.SetData(0, new GH_Command(command));
        }
    }

    public class Message : GH_Component
    {
        public Message() : base("Message", "Message", "Sends a text message to the teach pendant", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{CFAABB24-CAEE-49FC-850F-BE9F70F070CA}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconMessage;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Message", "M", "Message to display in teach pendant", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new CommandParameter(), "Command", "C", "Command", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string message = string.Empty;

            if (!DA.GetData(0, ref message)) { return; }

            var command = new Robots.Commands.Message(message);
            DA.SetData(0, new GH_Command(command));
        }
    }

    // Create Trigg Component for ABB
    public class Trigg : GH_Component
    {
        public Trigg() : base("Trigg", "T", "Creates a trigger command for use with ABB robots", "Robots", "Commands") { }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        public override Guid ComponentGuid => new Guid("{8785e050-502b-45c7-8e8b-e6dd84c25e93}");
        protected override System.Drawing.Bitmap Icon => Properties.Resources.iconCustomCommand; // Trigg Icon - need to create one

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            Params.RegisterInputParam(parameters[2]);
            AddParam(1);
            AddParam(2);
            AddParam(3);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new TriggParameter(), "Trigg", "Tr", "Trigg", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool hasTriggSpeed = Params.Input.Any(x => x.Name == "TriggSpeed");
            bool hasTriggIO = Params.Input.Any(x => x.Name == "TriggIO");
            bool hasTriggEquip = Params.Input.Any(x => x.Name == "TriggSpeed");

            // Shared Vars
            //bool hasDistance = Params.Input.Any(x => x.Name == "Distance");
            bool hasAOp = Params.Input.Any(x => x.Name == "AnalogOut");
            bool hasDOp = Params.Input.Any(x => x.Name == "DigitalOutput");
            bool hasGOp = Params.Input.Any(x => x.Name == "GroupDigitalOutput");
            bool hasSetValue = Params.Input.Any(x => x.Name == "SetValue");
            //bool hasVariableName = Params.Input.Any(x => x.Name == "NameOfTriggDataVar");

            // TriggSpeed Vars
            bool hasScaleLag = Params.Input.Any(x => x.Name == "ScaleLag");
            bool hasScaleValue = Params.Input.Any(x => x.Name == "ScaleValue");
            bool hasDipLag = Params.Input.Any(x => x.Name == "DipLag");

            // TriggIO Vars
            bool hasTime = Params.Input.Any(x => x.Name == "Time");

            // Trigg Equip Vars
            bool hasEquipLag = Params.Input.Any(x => x.Name == "EquipLag");

            GH_String triggdata_var_name = null;
            // check data types later
            double Distance = null;
            GH_String AOp = null;
            GH_String DOp = null;
            GH_String GOp = null;
            double SetValue = null;
            GH_String triggdata_name = null;
            double ScaleLag = null;
            dobule ScaleValue = null;
            double DipLag = null;
            double Time = null;
            double EquipLag = null;

            if (hasTriggSpeed)
            {

            }

            if (hasTriggIO)
            {

            }

            if (hasTriggEquip)
            {

            }

            if (hasSetValue)
            {

            }

            if (hasAOp)
            {

            }

            if (hasDOp)
            {

            }

            if (hasGOp)
            {

            }

            if (hasScaleLag)
            {

            }

            if (hasScaleValue)
            {

            }

            if (hasDipLag)
            {

            }

            if (hasTime)
            {

            }

            if (hasEquipLag)
            {

            }


        }

        // Variable inputs

        IGH_Param[] parameters = new IGH_Param[11]
        {
            // check param names etc
             new Param_Double() { Name = "Distance", NickName = "D", Description = "Distance from end point trigger occurs", Optional = false },
             new Param_String() { Name = "TriggData Name", NickName = "TrD", Description = "Name of TriggData Var", Optional = false },
             new Param_String() { Name = "Digital Output", NickName = "DO", Description = "Name of Digital Output for Trigg Command", Optional = true },
             new Param_String() { Name = "Analog Output", NickName = "AO", Description = "Name of Analog Output for Trigg Command", Optional = true },
             new Param_String() { Name = "Group Output", NickName = "DO", Description = "Name of Group Digital Output for Trigg Command", Optional = true },
             new Param_Double() { Name = "EquipLag", NickName = "El", Description = "Lag Time of external equipment (TriggEquip)", Optional = true },
             new Param_Double() { Name = "ScaleLag", NickName = "Sl", Description = "Lag Time of the robot to change the scaled output (TriggSpeed)", Optional = true },
             new Param_Double() { Name = "ScaleValue", NickName = "Sv", Description = "Scale value for the scaled output (scale_value * actual TCP speed in mm/s) (TriggSpeed)", Optional = true },
             new Param_Double() { Name = "DipLag", NickName = "Dl", Description = "Lag Time for scaled output when robot changes speed. (TriggSpeed)", Optional = true },
             new Param_Double() { Name = "SetValue", NickName = "V", Description = "Value to set at trigger time. (TriggIO, TriggEquip)", Optional = true },
             new Param_Double() { Name = "Time", NickName = "Tm", Description = "Time before or after trigger to set IO. (TriggIO)", Optional = true },

        };

        bool isTriggIO = true;
        bool isTriggEquip = false;
        bool isTriggSpeed = false;

        public override bool Write(GH_IWriter writer)
        {
            //writer.SetBoolean("IsCartesian", isCartesian);
            //return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            //isCartesian = reader.GetBoolean("IsCartesian");
            //return base.Read(reader);
        }



        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            // 1 only
            Menu_AppendItem(menu, "TriggIO", SwitchTriggCmd, true, true);
            Menu_AppendItem(menu, "TriggEquip", SwitchTriggCmd, true, true);
            Menu_AppendItem(menu, "TriggSpeed", SwitchTriggCmd, true, true);
            Menu_AppendSeparator(menu);
            // Common - don't need these 3 since they'll always be there
            ////Menu_AppendItem(menu, "Distance", ??, true, true, Params.Input.Any(x => x.Name == "Distance"));
            Menu_AppendItem(menu, "SetValue", AddSetValue, true, true, Params.Input.Any(x => x.Name == "SetValue"));
            ////Menu_AppendItem(menu, "TriggData", ??, true, true, Params.Input.Any(x => x.Name == "TriggData"));
            //Menu_AppendSeparator(menu);
            // these need to be set to 1 only
            Menu_AppendItem(menu, "DO", AddDO, true, true, Params.Input.Any(x => x.Name == "DO"));
            Menu_AppendItem(menu, "AO", AddAO, true, true, Params.Input.Any(x => x.Name == "AO"));
            Menu_AppendItem(menu, "GO", AddGO, true, true, Params.Input.Any(x => x.Name == "GO"));
            // TriggIO
            Menu_AppendItem(menu, "Time", AddTime, isTriggIO, Params.Input.Any(x => x.Name == "Time"));
            //Menu_AppendSeparator(menu);
            // TriggEquip
            //Menu_AppendItem(menu, "EquipLag", ??, true, isTriggEquip, Params.Input.Any(x => x.Name == "EquipLag"));
            Menu_AppendSeparator(menu);
            // TriggSpeed
            //Menu_AppendItem(menu, "Scale Lag", ??, isTriggSpeed, Params.Input.Any(x => x.Name == "ScaleLag"));
            //Menu_AppendItem(menu, "Scale Value", ??, isTriggSpeed, Params.Input.Any(x => x.Name == "ScaleValue"));
            Menu_AppendItem(menu, "Dip Lag", AddDipLag, isTriggSpeed, Params.Input.Any(x => x.Name == "Dip Lag"));

        }

        // Varible methods

        private void SwitchTriggType()
        {
            // ensure not duplicating
            if (isTriggIO)
            {
                isTriggEquip = false;
                isTriggSpeed = false;
            }
            else if (isTriggEquip)
            {
                isTriggIO = false;
                isTriggSpeed = false;
            }
            else
            {
                isTriggEquip = false;
                isTriggIO = false;
                isTriggSpeed = true;
            }


            if (isTriggIO)
            {
                Params.UnregisterInputParameter(Params.Input.FirstOrDefault(x => x.Name == "Plane"), true);
                Params.UnregisterInputParameter(Params.Input.FirstOrDefault(x => x.Name == "RobConf"), true);
                Params.UnregisterInputParameter(Params.Input.FirstOrDefault(x => x.Name == "Motion"), true);
                AddParam(3);
                AddParam(10);
            }
            else if (isTriggEquip)
            {
                Params.UnregisterInputParameter(Params.Input.FirstOrDefault(x => x.Name == "Joints"), true);
                AddParam(3);
                AddParam(6);

            }
            else if (isTriggSpeed)
            {
                AddParam(7);
                AddParam(8);
            }

            Params.OnParametersChanged();
            ExpireSolution(true);
        }
        private void SwitchTriggTypeEvent(object sender, EventArgs e) => SwitchTriggType();

        private void AddParam(int index)
        {
            IGH_Param parameter = parameters[index];

            if (Params.Input.Any(x => x.Name == parameter.Name))
                Params.UnregisterInputParameter(Params.Input.First(x => x.Name == parameter.Name), true);
            else
            {
                int insertIndex = Params.Input.Count;
                for (int i = 0; i < Params.Input.Count; i++)
                {
                    int otherIndex = Array.FindIndex(parameters, x => x.Name == Params.Input[i].Name);
                    if (otherIndex > index)
                    {
                        insertIndex = i;
                        break;
                    }
                }
                Params.RegisterInputParam(parameter, insertIndex);
            }
            Params.OnParametersChanged();
            ExpireSolution(true);
        }

        private void AddSetValue(object sender, EventArgs e) => AddParam(10);
        private void AddDO(object sender, EventArgs e) => AddParam(3);
        private void AddAO(object sender, EventArgs e) => AddParam(4);
        private void AddGO(object sender, EventArgs e) => AddParam(5);
        private void AddTime(object sender, EventArgs e) => AddParam(11);
        private void AddDipLag(object sender, EventArgs e) => AddParam(9);



        bool IGH_VariableParameterComponent.CanInsertParameter(GH_ParameterSide side, int index) => false;
        bool IGH_VariableParameterComponent.CanRemoveParameter(GH_ParameterSide side, int index) => false;
        IGH_Param IGH_VariableParameterComponent.CreateParameter(GH_ParameterSide side, int index) => null;
        bool IGH_VariableParameterComponent.DestroyParameter(GH_ParameterSide side, int index) => false;
        void IGH_VariableParameterComponent.VariableParameterMaintenance() { }
    }
}
using Expressium.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.ObjectRepositories
{
    public class ObjectRepositoryPage
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Base { get; set; }
        public bool Model { get; set; }

        public string NameLocator { get; set; }
        public string ControlsLocator { get; set; }
        public string IncludeControlsLocator { get; set; }

        public List<ObjectRepositorySynchronizer> Synchronizers { get; set; }
        public List<ObjectRepositoryMember> Members { get; set; }
        public List<ObjectRepositoryControl> Controls { get; set; }

        public ObjectRepositoryPage()
        {
            Model = false;

            Controls = new List<ObjectRepositoryControl>();
            Synchronizers = new List<ObjectRepositorySynchronizer>();
            Members = new List<ObjectRepositoryMember>();
        }

        public ObjectRepositoryPage Copy()
        {
            var copy = new ObjectRepositoryPage();
            copy.Assign(this);
            return copy;
        }

        public void Assign(ObjectRepositoryPage page)
        {
            Url = page.Url;
            Title = page.Title;
            Name = page.Name;
            Base = page.Base;
            Model = page.Model;

            NameLocator = page.NameLocator;
            ControlsLocator = page.ControlsLocator;
            IncludeControlsLocator = page.IncludeControlsLocator;

            Synchronizers.Clear();
            foreach (var synchronizer in page.Synchronizers)
                Synchronizers.Add(synchronizer.Copy());

            Members.Clear();
            foreach (var member in page.Members)
                Members.Add(member.Copy());

            Controls.Clear();
            foreach (var control in page.Controls)
                Controls.Add(control.Copy());
        }

        public bool IsSynchronizerAdded(ObjectRepositorySynchronizer synchronizer)
        {
            return Synchronizers.Any(m => m.GetHashCode() == synchronizer.GetHashCode());
        }

        public void AddSynchronizer(ObjectRepositorySynchronizer Synchronizer)
        {
            if (!IsSynchronizerAdded(Synchronizer))
                Synchronizers.Add(Synchronizer);
        }

        public ObjectRepositorySynchronizer GetSynchronizer(ObjectRepositorySynchronizer synchronizer)
        {
            if (IsSynchronizerAdded(synchronizer))
                return Synchronizers.FirstOrDefault(c => c.GetHashCode() == synchronizer.GetHashCode());

            return null;
        }

        public void DeleteSynchronizer(ObjectRepositorySynchronizer synchronizer)
        {
            if (IsSynchronizerAdded(synchronizer))
            {
                int index = Synchronizers.FindIndex(m => m.GetHashCode() == synchronizer.GetHashCode());
                Synchronizers.RemoveAt(index);
            }
        }

        public bool IsMemberAdded(ObjectRepositoryMember member)
        {
            return Members.Any(m => m.GetHashCode() == member.GetHashCode());
        }

        public void AddMember(ObjectRepositoryMember member)
        {
            if (!IsMemberAdded(member))
                Members.Add(member);
        }

        public ObjectRepositoryMember GetMember(ObjectRepositoryMember member)
        {
            if (IsMemberAdded(member))
                return Members.FirstOrDefault(c => c.GetHashCode() == member.GetHashCode());

            return null;
        }

        public void DeleteMember(ObjectRepositoryMember member)
        {
            if (IsMemberAdded(member))
            {
                int index = Members.FindIndex(m => m.GetHashCode() == member.GetHashCode());
                Members.RemoveAt(index);
            }
        }

        public bool IsControlAdded(ObjectRepositoryControl control)
        {
            return Controls.Any(m => m.GetHashCode() == control.GetHashCode());
        }

        public void AddControl(ObjectRepositoryControl control)
        {
            if (!IsControlAdded(control))
                Controls.Add(control);
        }

        public ObjectRepositoryControl GetControl(ObjectRepositoryControl control)
        {
            if (IsControlAdded(control))
                return Controls.FirstOrDefault(c => c.GetHashCode() == control.GetHashCode());

            return null;
        }

        public void DeleteControl(ObjectRepositoryControl control)
        {
            if (IsControlAdded(control))
            {
                foreach (var synchronizer in Synchronizers)
                {
                    if (synchronizer.Using == control.Name)
                    {
                        if (synchronizer.How == SynchronizerTypes.WaitForPageElementIsVisible.ToString() ||
                            synchronizer.How == SynchronizerTypes.WaitForPageElementIsEnabled.ToString())
                        {
                            var subItem = Synchronizers.Find(x => x.Using == control.Name);
                            Synchronizers.Remove(subItem);
                            break;
                        }
                    }
                }

                int index = Controls.FindIndex(m => m.GetHashCode() == control.GetHashCode());
                Controls.RemoveAt(index);
            }
        }

        public void RenameControl(ObjectRepositoryControl control, string newname)
        {
            if (IsControlAdded(control))
            {
                foreach (var synchronizer in Synchronizers)
                {
                    if (synchronizer.Using == control.Name)
                    {
                        if (synchronizer.How == SynchronizerTypes.WaitForPageElementIsVisible.ToString() ||
                            synchronizer.How == SynchronizerTypes.WaitForPageElementIsEnabled.ToString())
                            synchronizer.Using = newname;
                    }
                }

                control.Name = newname;
            }
        }

        public void SwapSynchronizers(int masterId, int slaveId)
        {
            var synchronizer = Synchronizers[masterId];
            Synchronizers[masterId] = Synchronizers[slaveId];
            Synchronizers[slaveId] = synchronizer;
        }

        public void SwapMembers(int masterId, int slaveId)
        {
            var member = Members[masterId];
            Members[masterId] = Members[slaveId];
            Members[slaveId] = member;
        }

        public void SwapControls(int masterId, int slaveId)
        {
            var control = Controls[masterId];
            Controls[masterId] = Controls[slaveId];
            Controls[slaveId] = control;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("The ObjectRepositoryPage property 'Name' is undefined...");

            foreach (var synchronizer in Synchronizers)
                synchronizer.Validate();

            foreach (var member in Members)
                member.Validate();

            foreach (var control in Controls)
                control.Validate();
        }

        public override int GetHashCode()
        {
            return string.Concat(Name).GetHashCode();
        }

        public void RenameUsage(string name, string newname)
        {
            if (Base == name)
                Base = newname;

            foreach (var control in Controls)
            {
                if (control.Target == name)
                    control.Target = newname;
            }

            foreach (var member in Members)
            {
                if (member.Page == name)
                    member.Page = newname;
            }
        }

        public void DeleteUsage(string name)
        {
            if (Base == name)
                Base = null;

            foreach (var control in Controls)
            {
                if (control.Target == name)
                    control.Target = null;
            }

            foreach (var member in Members)
            {
                if (member.Page == name)
                {
                    var subItem = Members.Find(x => x.Page == name);
                    Members.Remove(subItem);
                    break;
                }
            }
        }

        public bool HasFillFormControls()
        {
            foreach (var control in Controls)
            {
                if (control.IsFillFormControl())
                    return true;
            }

            return false;
        }

        public int GetNumberOfFillFormControls()
        {
            int numberOfControls = 0;

            foreach (var control in Controls)
            {
                if (control.IsFillFormControl())
                    numberOfControls++;
            }

            return numberOfControls;
        }
    }
}

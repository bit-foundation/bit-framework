using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;

namespace Bit.Client.Web.BlazorUI.Playground.Web.Pages
{
    public partial class Component
    {
        private bool CheckBoxOnChangedValue = false;
        private bool IsCheckBoxChecked = false;
        private bool IsCheckBoxIndeterminate = true;
        private bool IsCheckBoxIndeterminateInCode = true;
        private bool IsToggleChecked = true;
        private bool IsToggleUnChecked = false;

        private bool IsMessageBarHidden = false;
        private TextFieldType InputType = TextFieldType.Password;

        private int RatingBoundValue = 2;
        private int RatingLargeValue = 3;
        private int RatingSmallValue = 4;
        private int RatingReadOnlyValue = 5;
        private int RatingOutsideValue = 5;

        private double BasicSpinButtonValue = 5;
        private double BasicSpinButtonDisableValue = 20;
        private double SpinButtonWithCustomHandlerValue = 14;
        private double SpinButtonWithLabelAboveValue = 7;

        public string SelectedColor { get; set; } = "rgb(243,33,105,0.30)";

        private void HandleSpinButtonValueChange(double value)
        {
            SpinButtonWithCustomHandlerValue = value;
        }

        private readonly List<BitNavLinkItem> BasicNavLinks = new()
        {
            new BitNavLinkItem
            {
                Name = "Home",
                Url = "http://example.com",
                Key = "key1",
                Title = "Home",
                Target = "_blank",
                IsExpanded = true,
                CollapseAriaLabel = "Collapse Home section",
                Links = new List<BitNavLinkItem>
                        {
                            new BitNavLinkItem{
                                Name= "Activity",
                                Url= "http://msn.com",
                                Key= "key1-1",
                                Title="Activity",
                                Links=new List<BitNavLinkItem>
                                {
                                    new BitNavLinkItem{ Name= "Activity",Title= "Activity",Url= "http://msn.com",Key= "key1-1-1"},
                                    new BitNavLinkItem{ Name= "MSN",Title= "MSN", Url= "http://msn.com", Key= "key1-1-2",Disabled=true}
                                }
                            },
                            new BitNavLinkItem{Name= "MSN",Title="MSN", Url= "http://msn.com", Key= "key1-2",Disabled=true },
                        }
            },
            new BitNavLinkItem { Name = "Documents", Title = "Documents", Url = "http://example.com", Key = "key2", Target = "_blank", IsExpanded = true },
            new BitNavLinkItem { Name = "Pages", Title = "Pages", Url = "http://msn.com", Key = "key3", Target = "_parent" },
            new BitNavLinkItem { Name = "Notebook", Title = "Notebook", Url = "http://msn.com", Key = "key4", Disabled = true },
            new BitNavLinkItem { Name = "Communication and Media", Title = "Communication and Media", Url = "http://msn.com", Key = "key5", Target = "_top" },
            new BitNavLinkItem { Name = "News", Title = "News", Url = "http://msn.com", Key = "key6", Icon = "News", Target = "_self" },
        };

        private readonly List<BitNavLinkItem> BasicNoToolTipNavLinks = new()
        {
            new BitNavLinkItem
            {
                Name = "Home",
                Url = "http://example.com",
                Key = "key1",
                Target = "_blank",
                IsExpanded = true,
                CollapseAriaLabel = "Collapse Home section",
                Links = new List<BitNavLinkItem>
                        {
                            new BitNavLinkItem{
                                Name= "Activity",
                                Url= "http://msn.com",
                                Key= "key1-1",
                                Links=new List<BitNavLinkItem>
                                {
                                    new BitNavLinkItem{Name= "Activity",Url= "http://msn.com",Key= "key1-1-1" },
                                    new BitNavLinkItem{Name= "MSN", Url= "http://msn.com", Key= "key1-1-2",Disabled=true }
                                } },
                            new BitNavLinkItem{Name= "MSN", Url= "http://msn.com", Key= "key1-2",Disabled=true },
                        }
            },
            new BitNavLinkItem { Name = "Shared Documents and Files", Url = "http://example.com", Key = "key2", Target = "_blank", IsExpanded = true },
            new BitNavLinkItem { Name = "Pages", Url = "http://msn.com", Key = "key3", Target = "_parent" },
            new BitNavLinkItem { Name = "Notebook", Url = "http://msn.com", Key = "key4", Disabled = true },
            new BitNavLinkItem { Name = "Communication and Media", Url = "http://msn.com", Key = "key5", Target = "_top" },
            new BitNavLinkItem { Name = "News", Url = "http://msn.com", Key = "key6", Icon = "News", Target = "_self" },
        };

        private readonly List<BitNavLinkItem> BasicNoUrlNavLinks = new()
        {
            new BitNavLinkItem
            {
                Name = "Basic components",
                Key = "Key1",
                CollapseAriaLabel = "Collapse Basic components section",
                IsExpanded = true,
                Links = new List<BitNavLinkItem>
                        {
                            new BitNavLinkItem{Name= "ActivityItem", Key= "ActivityItem",Url="#/examples/activityitem" },
                            new BitNavLinkItem{Name= "Breadcrumb", Key= "Breadcrumb",Url="#/examples/breadcrumb" },
                            new BitNavLinkItem{Name= "Button", Key= "Button",Url="#/examples/button" }
                        }
            },
            new BitNavLinkItem
            {
                Name = "Extended components",
                Key = "Key2",
                CollapseAriaLabel = "Collapse Extended components section",
                IsExpanded = true,
                Links = new List<BitNavLinkItem>
                        {
                            new BitNavLinkItem{Name= "ColorPicker", Key= "ColorPicker",Url="#/examples/colorpicker" },
                            new BitNavLinkItem{Name= "ExtendedPeoplePicker", Key= "ExtendedPeoplePicker",Url="#/examples/extendedpeoplepicker" },
                            new BitNavLinkItem{Name= "GroupedList", Key= "GroupedList",Url="#/examples/groupedlist" }
                        }
            },
            new BitNavLinkItem
            {
                Name = "Utilities",
                Key = "Key3",
                CollapseAriaLabel = "Collapse Utilities section",
                IsExpanded = true,
                Links = new List<BitNavLinkItem>
                        {
                            new BitNavLinkItem{Name= "FocusTrapZone", Key= "FocusTrapZone",Url="#/examples/focustrapzone" },
                            new BitNavLinkItem{Name= "FocusZone", Key= "FocusZone",Url="#/examples/focuszone" },
                            new BitNavLinkItem{Name= "MarqueeSelection", Key= "MarqueeSelection",Url="#/examples/marqueeselection" }
                        }
            },
        };

        private readonly List<BitNavLinkItem> NestedLinks = new()
        {
            new BitNavLinkItem
            {
                Name = "Parent link 1",
                Url = "http://example.com",
                Key = "Key1",
                Title = "Parent link 1",
                CollapseAriaLabel = "Collapse Parent link 1",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem
                    {
                        Name= "Child link 1",
                        Url= "http://msn.com",
                        Key= "Key1-1",
                        Title="Child link 1",
                        Links=new List<BitNavLinkItem>
                        {
                            new BitNavLinkItem{Name= "3rd level link 1",Title="3rd level link 1",Url= "http://msn.com",Key= "Key1-1-1" },
                            new BitNavLinkItem{Name= "3rd level link 2",Title="3rd level link 2", Url= "http://msn.com", Key= "Key1-1-2",Disabled=true }
                        }
                    },
                    new BitNavLinkItem{Name = "Child link 2", Title = "Child link 2", Url = "http://msn.com", Key = "Key1-2" },
                    new BitNavLinkItem{Name = "Child link 3", Title = "Child link 3", Url = "http://msn.com", Key = "Key1-3", Disabled = true },
                }
            },
            new BitNavLinkItem
            {
                Name = "Parent link 2",
                Title = "Parent link 2",
                Url = "http://example.com",
                Key = "Key2",
                CollapseAriaLabel = "Collapse Parent link 2",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem{Name= "Child link 4", Title= "Child link 4", Url= "http://example.com", Key= "Key2-1" }
                }
            }
        };

        private readonly List<BitNavLinkItem> CustomHeaderLinks = new()
        {
            new BitNavLinkItem
            {
                Name = "Pages",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem{Name= "Activity", Url= "http://msn.com", Key= "Key1-1", Title="Activity" },
                    new BitNavLinkItem{Name= "News",Title="News", Url= "http://msn.com", Key= "Key1-2" },
                }
            },
            new BitNavLinkItem
            {
                Name = "More pages",
                Links = new List<BitNavLinkItem>
                {
                    new BitNavLinkItem{Name= "Settings", Title= "Settings", Url= "http://example.com", Key= "Key2-1" },
                    new BitNavLinkItem{Name= "Notes", Title= "Notes", Url= "http://example.com", Key= "Key2-1" }
                }
            }
        };

        #region PivotSamples

        public string OverridePivotSelectedKey { get; set; } = "1";
        public string SelectedPivotItemKey { get; set; } = "1";
        public BitPivotItem BitPivotItem { get; set; }
        public ComponentVisibility PivotItemVisibility { get; set; }
        public BitPivotItem SelectedPivotKey { get; set; } = new BitPivotItem { ItemKey = "Foo" };

        public void PivotSelectedKeyChanged(string key)
        {
            OverridePivotSelectedKey = key;
        }

        public void TogglePivotItemVisobility()
        {
            PivotItemVisibility = PivotItemVisibility == ComponentVisibility.Visible ? ComponentVisibility.Collapsed : ComponentVisibility.Visible;
        }

        #endregion 

        private bool TogleButtonChecked = true;
        private bool TogleButtonChecked2 = true;
        private bool TogleButtonTwoWayValue = true;
        private bool TogleButtonTwoWayValue2 = true;
        private bool OnToggleButtonChanged;
        private void ToggleButtonChanged(bool newValue)
        {
            OnToggleButtonChanged = newValue;
        }

        private void HideMessageBar(MouseEventArgs args)

        {
            IsMessageBarHidden = true;
        }
        private string RatingChangedText = "";

        private List<Person>[] people = new List<Person>[2];

        protected override void OnInitialized()
        {
            base.OnInitialized();

            //initialize people list
            Person person = new Person();
            people[0] = person.GetPeople(8000);
            people[1] = person.GetPeople(100);
        }

        private List<DropDownItem> GetDropdownItems()
        {
            List<DropDownItem> items = new();
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Header,
                Text = "Fruits"
            });
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Normal,
                Text = "Apple",
                Value = "f-app"
            });
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Normal,
                Text = "Orange",
                Value = "f-ora",
                IsEnabled = false
            });
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Normal,
                Text = "Banana",
                Value = "f-ban",
            });
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Divider,
            });
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Header,
                Text = "Vegetables"
            });
            items.Add(new DropDownItem()
            {
                ItemType = DropDownItemType.Normal,
                Text = "Broccoli",
                Value = "v-bro",
            });

            return items;
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Job { get; set; }

        public List<Person> GetPeople(int itemCount)
        {
            List<Person> people = new();
            for (int i = 0; i < itemCount; i++)
            {
                people.Add(new Person
                {
                    Id = i + 1,
                    FirstName = $"Person {(i + 1).ToString()}",
                    LastName = $"Person Family {(i + 1).ToString()}",
                    Job = $"Programmer {(i + 1).ToString()}"
                });
            }
            return people;
        }
    }
}

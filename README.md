# Custom Assets For Winforms
Adds a series of useful controls to Winforms.
Before starting, make sure you download the **dlls** that correspond to the controls you want to use and their respective **dependencies**. To make sure you have everything, use the **diagram** below.

![Dependency Diagram](./.main/DependencyDiagram.jpg)

## Item Datas
If a control **handles data** in any way, it's going to do that through the `Commons::ItemDatas` class. To make your own data type that's compatible with this framework, simply extend said class and add your own fields.

```csharp
public class DataExample : ItemDatas
    {
        public DataExample() : base()
        {
            Value = "";
        }

        public String Value { get; set; }
    }
```

## Styles
Every component that can be visualized has a style property that accepts an instance of the `Commons::Style` class. And their appearance can be controlled by simply updating said property.

Furthermore, some **static methods** are provided to control the look of some **default Winforms controls**. Simply provide your control and your style and it'll wrap it up nicely for you.

For example, you could have all your style instances as **static readonly fields** and store them in a class named something like "Styles".

```csharp
public static class Styles
    {
        public static readonly Style LIGHT_MODE = new(
            SystemColors.Control,
            SystemColors.ControlDark,
            "Segoe UI",
            SystemColors.ControlText,
            Color.Teal,
            SystemColors.ControlDark,
            "Segoe UI",
            SystemColors.ControlText,
            Color.Teal,
            FlatStyle.Standard
            );

        public static readonly Style DARK_MODE = new(
            Color.FromArgb(255, 30, 30, 30),
            Color.FromArgb(255, 51, 51, 51),
            "Segoe UI",
            SystemColors.HighlightText,
            Color.Teal,
            Color.FromArgb(255, 51, 51, 51),
            "Segoe UI",
            SystemColors.HighlightText,
            Color.Teal,
            FlatStyle.Standard
            );
    }
```

So that changing the appearance of a control is as easy as typing somethis like `control.Style = Styles.DARK_MODE`.

(Or `control.Style = Styles.LIGHT_MODE` if you're a maniac!)

## Rest Client
This framework also provides a nicer way to make **http** or **https** requests. First of all, you'll need to create your an instance of the `RestClient::Request` class and specify the parameters.

```csharp
public static class Requests
    {
        static Requests()
        {
            String UNIS_BY_COUNTRY_BASE_URL = "http://universities.hipolabs.com";

            SEARCH_UNIS_BY_COUNTRY = new (UNIS_BY_COUNTRY_BASE_URL, "/search", new Arg[] { new("country") },
                "{\"mode\": \"Test\"}");
        }

        public static readonly Request SEARCH_UNIS_BY_COUNTRY;
    }
```

`RestClient::Arg` is a class that allows you to define the arguments accepted by the request. If you use the constructor with **one parameter**, it means you want to create an arg with the name provided, of which the value will be specified when you actually make the request. If you use the one with **two parameters**, you are actually creating a `new Arg(String name, String value)` that is constant.

Now, whenever you need it, simply call the MakeRequest method and access it's result by reading the Result property of the task that is returned.

```csharp
String res = RestClient.HttpClient.MakeRequest(request,
                new String[] { textBoxQuery.Text.Replace(" ", "+") }).Result;
```

## SearchBar
This control uses the **Http Requests Framework** discussed earlier to implement an extremely easy to use **search bar**. All you need to do is drag and drop it in any form, customize any of it's properties and supply it with a compatible `RestClient::Request` instance. That is, a request with **one arg** containing the search query.

```csharp
customSearchBar.Request = Requests.SEARCH_UNIS_BY_COUNTRY;
```

## Item Managers
Here, you will find everything you need to create forms that will **manage the data classes** you've created by extending `Commons::ItemDatas`.

Simply create a new form which extends `CustomItemManagers::FieldsForm` and then compose it by using the **fields provided** (as well as some default elements, like buttons or labels):

1. `CustomItemManagers::TextField`
2. `CustomItemManagers::CopyableTextField`
3. `CustomItemManagers::TextFieldEditor`
4. `CustomItemManagers::PathFieldEditor`

Also, don't forget to **override** `CustomItemManagers::FieldsForm.Populate()`, to control how the data info get displayed in the form, and `CustomItemManagers::FieldsForm.ApplyStyle()`, so you can control how the form transforms when it's style is changed.

```csharp
public partial class ExampleViewer : FieldsForm
    {
        public ExampleViewer()
        {
            InitializeComponent();
        }

        protected override void Populate()
        {
            base.Populate();

            if (ItemDatas is null)
            {
                fieldValue.Value = "(Null Object)";
                return;
            }

            if (ItemDatas is not DataExample)
            {
                fieldValue.Value = "(Incompatible Class)";
                return;
            }

            DataExample dataExample = (DataExample)ItemDatas;

            if (dataExample.Value is null)
            {
                fieldValue.Value = "(Null Field)";
            }
            else
            {
                fieldValue.Value = dataExample.Value;
            }
        }

        protected override void ApplyStyle()
        {
            base.ApplyStyle();

            fieldValue.Style = Style;
        }
    }
```

# Items List
Implements an extremely easy to use **list control** that displays a set of data of a class created, as always, by extending `Commons::ItemDatas`.

Before we talk about the list itself, we need to create the item that will be actually displayed by it. To do that, simply create a new control and **extend** `CustomLists::ListItem`. Next, design your control as you please and **override** `CustomLists::ListItem.Populate()` and `CustomLists::ListItem.ApplyStyle()`. Also, unless you want to have a specific behavour, remember to add the **ListItem_Click** consumer to the click event of all the controls in the class.

```csharp
public partial class ExampleItem : ListItem
    {
        public ExampleItem()
        {
            InitializeComponent();

            txtValue.Click += ListItem_Click;
        }

        protected override void Populate()
        {
            base.Populate();

            if (ItemDatas is null)
            {
                txtValue.Text = "Value: (Null Object)";
                return;
            }

            if (ItemDatas is not DataExample)
            {
                txtValue.Text = "Value: (Incompatible Class)";
                return;
            }

            DataExample dataExample = (DataExample)ItemDatas;

            if (dataExample.Value is null)
            {
                txtValue.Text = "Value: (Null Field)";
            }
            else
            {
                txtValue.Text = "Value: " + dataExample.Value;
            }
        }

        protected override void ApplyStyle()
        {
            base.ApplyStyle();

            Style.Apply(txtValue, Style, FontStyle.Regular);
        }
    }
```

Now, drag and drop the `CustomLists::CustomList` control onto any form and set it up as you please.

If you want it, you can actually specify the form that you want to use to visualize the data and the one you want to use it to modify it. In order to do that, create two forms that **extend** `CustomItemManagers::FieldsForm` and provide them to the list by updating the right properties.

```csharp
customList.Viewer = typeof(ExampleViewer);
```

If an editor is provided, an **edit button** will be shown in the top-right corner of every ListItem. By clicking it, the edit screen will be displayed.

Finally, to set the data that is to be shown by the list, you can use `CustomLists::CustomList.SetItems<TItemDatas, TListItem>(List<TItemDatas> itemDatas) where TItemDatas : ItemDatas where TListItem : ListItem`.

```csharp
customList.SetItems<DataExample, ExampleItem>(GenPlaceHolderList(10));
```

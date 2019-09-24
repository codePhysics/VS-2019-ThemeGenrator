using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ThemeGenerator.Generator
{
	public class Generator
	{
		private string vsSettingsFile { get; }
		private string themeName { get; }
		private string _baseTheme;
		List<string> baseColorList;
		List<string> secondaryColorList;
		List<string> thirdColorList;
		string BaseThemePath;

		public string BaseTheme
		{
			set
			{
				if (value == Constants.Light)
				{
					this.baseColorList = new List<string> { "FFF5F5F5", "FFEEEEF2", "FFFFFFFF", };
					this.secondaryColorList = new List<string> { "FF717171", "FFDEDFE7", "FF3399FF", "FFF6F6F6", "FFCCCEDB" };
					this.thirdColorList = new List<string> { "FF3399FF", "FFC2C3C9" };
					this.BaseThemePath = @"C:\codePhysics\VS2017 Themes\BaseLight.vstheme";
				}
				else
				{
					this.baseColorList = new List<string> { "FF3E3E42", "FF2D2D30", "FF282828", "FF252526", "FF333337", "FFE2E2E2" };
					this.secondaryColorList = new List<string> { "FF393F4B", "FF555555", "FF686868", "FF007ACC", "FF1B1B1C", "FF3F3F46" };
					this.thirdColorList = new List<string> { "FF3399FF", "FF686868" };
					this.BaseThemePath = @"C:\codePhysics\VS2017 Themes\BaseDark.vstheme";
				}
				_baseTheme = value;
			}
		}

		public Generator(string filePath)
		{
			this.vsSettingsFile = filePath;
			this.themeName = ThemeInfo.ThemeName;
			this.BaseTheme = ThemeInfo.BaseTheme;
		}
		public void GenerateTheme()
		{
			Theme.Theme.Name = themeName;
			OverRideListColors();
			ApplyVSSettings();
			SetIndividualItems();
			Serialize();
		}
		void ApplyVSSettings()
		{
			var newColors = UserSetting.Category.Category.FontsAndColors.Categories.Category.Items;
			foreach (var element in newColors)
			{
				var categroy = Categories.Where(x => x.Name == element.Name);
				foreach (var item in categroy)
				{
					if (item.Background != null && element.Background != "0x02000000")
					{
						item.Background.Source = GetHexColorForVSSetting(element.Background);
					}
					if (item.Foreground != null && element.Foreground != "0x02000000")
					{
						item.Foreground.Type = "CT_RAW";
						item.Foreground.Source = GetHexColorForVSSetting(element.Foreground);
					}
				}
			}
		}

		void OverRideListColors()
		{

			// All remaining items
			foreach (var color in Categories)
			{
				if (color.Background != null)
				{
					if (baseColorList.Contains(color.Background.Source))
					{
						color.Background.Source = MainBackground;
					}
					if (secondaryColorList.Contains(color.Background.Source))
					{
						color.Background.Source = SecondaryColor;
					}
					if (thirdColorList.Contains(color.Background.Source))
					{
						color.Background.Source = ThirdColor;
					}
				}
			}
		}

		string GetColorVariant(string baseColor, float percentage)
		{
			baseColor = baseColor.Replace("FF", "#");
			var cc = new ColorConverter();
			var color = (Color)cc.ConvertFromString(baseColor);
			var newcolor = ControlPaint.Light(color, percentage);
			Color myColor = Color.FromArgb(color.R, color.G, color.B);
			string hex = "FF" + newcolor.R.ToString("X2") + newcolor.G.ToString("X2") + newcolor.B.ToString("X2");
			return hex;
		}
		void Serialize()
		{
			System.Xml.Serialization.XmlSerializer writer =
				new System.Xml.Serialization.XmlSerializer(typeof(Themes));

			var path = $@"C:\codePhysics\VS2017 Themes\{themeName}.vstheme";
			System.IO.FileStream file = System.IO.File.Create(path);

			writer.Serialize(file, Theme);
			file.Close();
		}
		string GetHexColorForVSSetting(string htmlColor)
		{
			var cc = new ColorConverter();
			var color = (Color)cc.ConvertFromString(htmlColor);
			Color myColor = Color.FromArgb(color.R, color.G, color.B);
			string hex = "FF" + myColor.B.ToString("X2") + myColor.G.ToString("X2") + myColor.R.ToString("X2");
			return hex;
		}
		#region Individual Items
		void SetIndividualItems()
		{
			SelectedItem();
			CurrentLineHighlight();
			OutlinigMargin();
			CollapsibleRegion();
			BraceMatching();
			RazorCode();
			SetTextEditorBackground();
		}
		void RazorCode()
		{
			Categories.Where(x => x.Name == "RazorCode").SingleOrDefault().Background.Source = MainBackground;
			Categories.Where(x => x.Name == "HTML Server-Side Script").SingleOrDefault().Background.Source = MainBackground;

			var selectedTabColorPickListPreference = new List<string> { "class name", "keyword" };
			var identifier = UserSetting.Category.Category.FontsAndColors.Categories.Category.Items
								.Where(x => selectedTabColorPickListPreference.Contains(x.Name.ToLower()) && x.Foreground != "0x02000000").FirstOrDefault();
			if (identifier != null)
			{
				Categories.Where(x => x.Name == "HTML Server-Side Script").SingleOrDefault().Foreground.Source = GetHexColorForVSSetting(identifier.Foreground);
			}
		}
		void BraceMatching()
		{
			Categories.Where(x => x.Name == "brace matching").SingleOrDefault().Background.Source = SecondaryColor;
		}
		void CollapsibleRegion()
		{
			var collapsibleRegion = Categories.Where(x => x.Name == "outlining.collapsehintadornment");
			foreach (var element in collapsibleRegion)
			{
				element.Background.Source = SecondaryColor;
			}
		}
		void SelectedItem()
		{
			var selectedTabColorPickListPreference = new List<string> { "identifier", "class name", "keyword" };
			var identifier = UserSetting.Category.Category.FontsAndColors.Categories.Category.Items
								.Where(x => selectedTabColorPickListPreference.Contains(x.Name.ToLower()) && x.Foreground != "0x02000000").FirstOrDefault();

			if (identifier != null)
			{
				var selectedItemBackground = GetHexColorForVSSetting(identifier.Foreground);
				Categories.SingleOrDefault(x => x.Name == "FileTabSelectedText").Background.Source = MainBackground;
				Categories.SingleOrDefault(x => x.Name == "FileTabButtonSelectedActiveGlyph").Background.Source = MainBackground;

				foreach (var item in Categories.Where(x => x.Name == "SelectedItemActive"))
				{
					item.Foreground.Source = MainBackground;
				}
				foreach (var item in Categories.Where(x => x.Name == "SelectedItemInactive"))
				{
					item.Background.Source = SecondaryColor;
				}
				var selectedItemBackgroundList = new List<string>() { "SelectedItemActive" };
				var selectedItem = Categories.Where(x => x.Name.Contains("FileTabSelectedGradient") || selectedItemBackgroundList.Contains(x.Name));
				foreach (var item in selectedItem)
				{
					item.Background.Source = selectedItemBackground;
				}
			}
		}
		void OutlinigMargin()
		{
			// Outlining Margin
			var outlineMargin = Categories.Where(x => x.Name == "outlining.verticalrule");
			foreach (var element in outlineMargin)
			{
				element.Foreground.Source = MainBackground;
			}
		}
		void CurrentLineHighlight()
		{
			// Current Line Highlight
			Categories.SingleOrDefault(x => x.Name == "CurrentLineActiveFormat").Foreground.Source = SecondaryColor;
			Categories.SingleOrDefault(x => x.Name == "CurrentLineActiveFormat").Foreground.Type = "CT_RAW";
		}

		void SetTextEditorBackground()
		{
			Categories.Where(x => x.Name == "Plain Text").ToList().ForEach(s =>
			{
				s.Background.Source = MainBackground;
				s.Background.Type = "CT_RAW";
			});

			//var editorBackground = Theme.Theme.Category.Where(x => x.Name.Contains("Text Editor") &&
			//			 x.Color.Any(s => s.Background.Source == "00000000")).SelectMany(x => x.Color);
			//editorBackground.ToList().ForEach(x =>
			//{
			//	x.Background.Type = "CT_RAW";
			//	x.Background.Source = MainBackground;
			//});
		}

		#endregion

		#region Helper Variables


		IEnumerable<ThemesThemeCategoryColor> Categories => Theme.Theme.Category.SelectMany(x => x.Color);

		string _mainBackground;
		public String MainBackground
		{
			get
			{
				if (string.IsNullOrEmpty(_mainBackground))
				{
					_mainBackground = GetHexColorForVSSetting(UserSetting.Category.Category.FontsAndColors.Categories.Category.Items.SingleOrDefault(x => x.Name == "Plain Text").Background);
				}
				return _mainBackground;
			}

		}

		string _secondaryColor;
		public String SecondaryColor
		{
			get
			{
				if (string.IsNullOrEmpty(_secondaryColor))
				{
					_secondaryColor = GetColorVariant(MainBackground, .15f);
				}
				return _secondaryColor;
			}
		}
		string _thirdColor;
		public String ThirdColor
		{
			get
			{
				if (string.IsNullOrEmpty(_thirdColor))
				{
					_thirdColor = GetColorVariant(MainBackground, .16f);
				}
				return _thirdColor;
			}
		}

		Themes _theme;
		Themes Theme
		{
			get
			{
				if (_theme == null)
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Themes));
					using (FileStream fileStream = new FileStream(BaseThemePath, FileMode.Open))
					{
						_theme = (Themes)serializer.Deserialize(fileStream);
					}
				}
				return _theme;
			}
		}

		UserSettings _userSetting;
		UserSettings UserSetting
		{
			get
			{
				XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
				using (FileStream fileStream = new FileStream(vsSettingsFile, FileMode.Open))
				{
					_userSetting = (UserSettings)serializer.Deserialize(fileStream);
				}
				return _userSetting;
			}
		}



		#endregion

		/********** HELPER CLASSES. DO NOT TOUCH***************/

		#region HELPER CLASSES


		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
		public partial class Themes
		{

			private ThemesTheme themeField;

			/// <remarks/>
			public ThemesTheme Theme
			{
				get
				{
					return this.themeField;
				}
				set
				{
					this.themeField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class ThemesTheme
		{

			private ThemesThemeCategory[] categoryField;

			private string nameField;

			private string gUIDField;

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("Category")]
			public ThemesThemeCategory[] Category
			{
				get
				{
					return this.categoryField;
				}
				set
				{
					this.categoryField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string GUID
			{
				get
				{
					return this.gUIDField;
				}
				set
				{
					this.gUIDField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class ThemesThemeCategory
		{

			private ThemesThemeCategoryColor[] colorField;

			private string nameField;

			private string gUIDField;

			/// <remarks/>
			[System.Xml.Serialization.XmlElementAttribute("Color")]
			public ThemesThemeCategoryColor[] Color
			{
				get
				{
					return this.colorField;
				}
				set
				{
					this.colorField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string GUID
			{
				get
				{
					return this.gUIDField;
				}
				set
				{
					this.gUIDField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class ThemesThemeCategoryColor
		{

			private ThemesThemeCategoryColorBackground backgroundField;

			private ThemesThemeCategoryColorForeground foregroundField;

			private string nameField;

			/// <remarks/>
			public ThemesThemeCategoryColorBackground Background
			{
				get
				{
					return this.backgroundField;
				}
				set
				{
					this.backgroundField = value;
				}
			}

			/// <remarks/>
			public ThemesThemeCategoryColorForeground Foreground
			{
				get
				{
					return this.foregroundField;
				}
				set
				{
					this.foregroundField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class ThemesThemeCategoryColorBackground
		{

			private string typeField;

			private string sourceField;

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Type
			{
				get
				{
					return this.typeField;
				}
				set
				{
					this.typeField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Source
			{
				get
				{
					return this.sourceField;
				}
				set
				{
					this.sourceField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class ThemesThemeCategoryColorForeground
		{

			private string typeField;

			private string sourceField;

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Type
			{
				get
				{
					return this.typeField;
				}
				set
				{
					this.typeField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Source
			{
				get
				{
					return this.sourceField;
				}
				set
				{
					this.sourceField = value;
				}
			}
		}

		// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
		public partial class UserSettings
		{

			private UserSettingsApplicationIdentity applicationIdentityField;

			private UserSettingsToolsOptions toolsOptionsField;

			private UserSettingsCategory categoryField;

			/// <remarks/>
			public UserSettingsApplicationIdentity ApplicationIdentity
			{
				get
				{
					return this.applicationIdentityField;
				}
				set
				{
					this.applicationIdentityField = value;
				}
			}

			/// <remarks/>
			public UserSettingsToolsOptions ToolsOptions
			{
				get
				{
					return this.toolsOptionsField;
				}
				set
				{
					this.toolsOptionsField = value;
				}
			}

			/// <remarks/>
			public UserSettingsCategory Category
			{
				get
				{
					return this.categoryField;
				}
				set
				{
					this.categoryField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsApplicationIdentity
		{

			private decimal versionField;

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public decimal version
			{
				get
				{
					return this.versionField;
				}
				set
				{
					this.versionField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsToolsOptions
		{

			private UserSettingsToolsOptionsToolsOptionsCategory toolsOptionsCategoryField;

			/// <remarks/>
			public UserSettingsToolsOptionsToolsOptionsCategory ToolsOptionsCategory
			{
				get
				{
					return this.toolsOptionsCategoryField;
				}
				set
				{
					this.toolsOptionsCategoryField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsToolsOptionsToolsOptionsCategory
		{

			private string nameField;

			private string registeredNameField;

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string RegisteredName
			{
				get
				{
					return this.registeredNameField;
				}
				set
				{
					this.registeredNameField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategory
		{

			private UserSettingsCategoryCategory categoryField;

			private string nameField;

			private string registeredNameField;

			/// <remarks/>
			public UserSettingsCategoryCategory Category
			{
				get
				{
					return this.categoryField;
				}
				set
				{
					this.categoryField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string RegisteredName
			{
				get
				{
					return this.registeredNameField;
				}
				set
				{
					this.registeredNameField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategoryCategory
		{

			private UserSettingsCategoryCategoryPropertyValue propertyValueField;

			private UserSettingsCategoryCategoryFontsAndColors fontsAndColorsField;

			private string nameField;

			private string categoryField;

			private string packageField;

			private string registeredNameField;

			private string packageNameField;

			/// <remarks/>
			public UserSettingsCategoryCategoryPropertyValue PropertyValue
			{
				get
				{
					return this.propertyValueField;
				}
				set
				{
					this.propertyValueField = value;
				}
			}

			/// <remarks/>
			public UserSettingsCategoryCategoryFontsAndColors FontsAndColors
			{
				get
				{
					return this.fontsAndColorsField;
				}
				set
				{
					this.fontsAndColorsField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Category
			{
				get
				{
					return this.categoryField;
				}
				set
				{
					this.categoryField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Package
			{
				get
				{
					return this.packageField;
				}
				set
				{
					this.packageField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string RegisteredName
			{
				get
				{
					return this.registeredNameField;
				}
				set
				{
					this.registeredNameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string PackageName
			{
				get
				{
					return this.packageNameField;
				}
				set
				{
					this.packageNameField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategoryCategoryPropertyValue
		{

			private string nameField;

			private byte valueField;

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlTextAttribute()]
			public byte Value
			{
				get
				{
					return this.valueField;
				}
				set
				{
					this.valueField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategoryCategoryFontsAndColors
		{

			private UserSettingsCategoryCategoryFontsAndColorsCategories categoriesField;

			private decimal versionField;

			/// <remarks/>
			public UserSettingsCategoryCategoryFontsAndColorsCategories Categories
			{
				get
				{
					return this.categoriesField;
				}
				set
				{
					this.categoriesField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public decimal Version
			{
				get
				{
					return this.versionField;
				}
				set
				{
					this.versionField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategoryCategoryFontsAndColorsCategories
		{

			private UserSettingsCategoryCategoryFontsAndColorsCategoriesCategory categoryField;

			/// <remarks/>
			public UserSettingsCategoryCategoryFontsAndColorsCategoriesCategory Category
			{
				get
				{
					return this.categoryField;
				}
				set
				{
					this.categoryField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategoryCategoryFontsAndColorsCategoriesCategory
		{

			private UserSettingsCategoryCategoryFontsAndColorsCategoriesCategoryItem[] itemsField;

			private string gUIDField;

			private string fontNameField;

			private byte fontSizeField;

			private byte charSetField;

			private string fontIsDefaultField;

			/// <remarks/>
			[System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
			public UserSettingsCategoryCategoryFontsAndColorsCategoriesCategoryItem[] Items
			{
				get
				{
					return this.itemsField;
				}
				set
				{
					this.itemsField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string GUID
			{
				get
				{
					return this.gUIDField;
				}
				set
				{
					this.gUIDField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string FontName
			{
				get
				{
					return this.fontNameField;
				}
				set
				{
					this.fontNameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public byte FontSize
			{
				get
				{
					return this.fontSizeField;
				}
				set
				{
					this.fontSizeField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public byte CharSet
			{
				get
				{
					return this.charSetField;
				}
				set
				{
					this.charSetField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string FontIsDefault
			{
				get
				{
					return this.fontIsDefaultField;
				}
				set
				{
					this.fontIsDefaultField = value;
				}
			}
		}

		/// <remarks/>
		[System.SerializableAttribute()]
		[System.ComponentModel.DesignerCategoryAttribute("code")]
		[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
		public partial class UserSettingsCategoryCategoryFontsAndColorsCategoriesCategoryItem
		{

			private string nameField;

			private string foregroundField;

			private string backgroundField;

			private string boldFontField;

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Name
			{
				get
				{
					return this.nameField;
				}
				set
				{
					this.nameField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Foreground
			{
				get
				{
					return this.foregroundField;
				}
				set
				{
					this.foregroundField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string Background
			{
				get
				{
					return this.backgroundField;
				}
				set
				{
					this.backgroundField = value;
				}
			}

			/// <remarks/>
			[System.Xml.Serialization.XmlAttribute()]
			public string BoldFont
			{
				get
				{
					return this.boldFontField;
				}
				set
				{
					this.boldFontField = value;
				}
			}
		}
		#endregion
	}
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Onllama.Tiny.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Onllama.Tiny.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ .System }}
        ///User: {{ .Prompt }}
        ///
        ///Assistant:
        ///&quot;&quot;&quot;
        ///PARAMETER num_ctx 4096 的本地化字符串。
        /// </summary>
        internal static string deekseekTmp {
            get {
                return ResourceManager.GetString("deekseekTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;{{ if .System }}{{ .System }}
        ///
        ///{{ end }}{{ if .Prompt }}User: {{ .Prompt }}
        ///
        ///{{ end }}Assistant: {{ .Response }}&quot;
        ///PARAMETER stop &quot;User:&quot;
        ///PARAMETER stop &quot;Assistant:&quot; 的本地化字符串。
        /// </summary>
        internal static string deekseekv2Tmp {
            get {
                return ResourceManager.GetString("deekseekv2Tmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;&lt;start_of_turn&gt;user
        ///{{ if .System }}{{ .System }} {{ end }}{{ .Prompt }}&lt;end_of_turn&gt;
        ///&lt;start_of_turn&gt;model
        ///{{ .Response }}&lt;end_of_turn&gt;
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;start_of_turn&gt;&quot;
        ///PARAMETER stop &quot;&lt;end_of_turn&gt;&quot;
        ///PARAMETER repeat_penalty 1 的本地化字符串。
        /// </summary>
        internal static string gemmaTmp {
            get {
                return ResourceManager.GetString("gemmaTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;[INST] {{ .System }} {{ .Prompt }} [/INST]&quot;&quot;&quot;
        ///PARAMETER stop &quot;[INST]&quot;
        ///PARAMETER stop &quot;[/INST]&quot; 的本地化字符串。
        /// </summary>
        internal static string mistralTmp {
            get {
                return ResourceManager.GetString("mistralTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ if .System }}&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;
        ///{{ end }}{{ if .Prompt }}&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///{{ end }}&lt;|im_start|&gt;assistant
        ///{{ .Response }}&lt;|im_end|&gt;
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string qwen2Tmp {
            get {
                return ResourceManager.GetString("qwen2Tmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ if .System }}&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;{{ end }}&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///&lt;|im_start|&gt;assistant
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string qwenTmp {
            get {
                return ResourceManager.GetString("qwenTmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-clipboard-fill&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path fill-rule=&quot;evenodd&quot; d=&quot;M10 1.5a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5v1a.5.5 0 0 0 .5.5h3a.5.5 0 0 0 .5-.5zm-5 0A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5v1A1.5 1.5 0 0 1 9.5 4h-3A1.5 1.5 0 0 1 5 2.5zm-2 0h1v1A2.5 2.5 0 0 0 6.5 5h3A2.5 2.5 0 0 0 12 2.5v-1h1a2 2 0 0 1 2 2V14a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V3.5a2 2 0 0 1 2-2&quot;/&gt;
        ///&lt;/svg&gt; 的本地化字符串。
        /// </summary>
        internal static string svgCopy {
            get {
                return ResourceManager.GetString("svgCopy", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-trash3-fill&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path d=&quot;M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string svgDel {
            get {
                return ResourceManager.GetString("svgDel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-info-circle-fill&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path d=&quot;M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16m.93-9.412-1 4.705c-.07.34.029.533.304.533.194 0 .487-.07.686-.246l-.088.416c-.287.346-.92.598-1.465.598-.703 0-1.002-.422-.808-1.319l.738-3.468c.064-.293.006-.399-.287-.47l-.451-.081.082-.381 2.29-.287zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2&quot;/&gt;
        ///&lt;/svg&gt; 的本地化字符串。
        /// </summary>
        internal static string svgInfo {
            get {
                return ResourceManager.GetString("svgInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-info-circle&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path d=&quot;M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14m0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16&quot;/&gt;
        ///  &lt;path d=&quot;m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0&quot;/&gt;
        ///&lt;/svg&gt; 的本地化字符串。
        /// </summary>
        internal static string svgInfoOutline {
            get {
                return ResourceManager.GetString("svgInfoOutline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-pin-angle-fill&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path d=&quot;M9.828.722a.5.5 0 0 1 .354.146l4.95 4.95a.5.5 0 0 1 0 .707c-.48.48-1.072.588-1.503.588-.177 0-.335-.018-.46-.039l-3.134 3.134a6 6 0 0 1 .16 1.013c.046.702-.032 1.687-.72 2.375a.5.5 0 0 1-.707 0l-2.829-2.828-3.182 3.182c-.195.195-1.219.902-1.414.707s.512-1.22.707-1.414l3.182-3.182-2.828-2.829a.5.5 0 0 1 0-.707c.688-.688 1.673-.767 2.375-.72a6 6 0 0 1 1.013.16l3.134- [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string svgPin {
            get {
                return ResourceManager.GetString("svgPin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg t=&quot;1710874848376&quot; class=&quot;icon&quot; viewBox=&quot;0 0 1024 1024&quot; version=&quot;1.1&quot; xmlns=&quot;http://www.w3.org/2000/svg&quot; p-id=&quot;8348&quot; xmlns:xlink=&quot;http://www.w3.org/1999/xlink&quot; width=&quot;200&quot; height=&quot;200&quot;&gt;&lt;path d=&quot;M924.8 625.7l-65.5-56c3.1-19 4.7-38.4 4.7-57.8s-1.6-38.8-4.7-57.8l65.5-56c10.1-8.6 13.8-22.6 9.3-35.2l-0.9-2.6c-18.1-50.5-44.9-96.9-79.7-137.9l-1.8-2.1c-8.6-10.1-22.5-13.9-35.1-9.5l-81.3 28.9c-30-24.6-63.5-44-99.7-57.6l-15.7-85c-2.4-13.1-12.7-23.3-25.8-25.7l-2.7-0.5c-52.1-9.4-106.9-9.4-159 0l-2.7 0.5c-13.1 2.4-23 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string svgSetting {
            get {
                return ResourceManager.GetString("svgSetting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-snow2&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path d=&quot;M8 16a.5.5 0 0 1-.5-.5v-1.293l-.646.647a.5.5 0 0 1-.707-.708L7.5 12.793v-1.086l-.646.647a.5.5 0 0 1-.707-.708L7.5 10.293V8.866l-1.236.713-.495 1.85a.5.5 0 1 1-.966-.26l.237-.882-.94.542-.496 1.85a.5.5 0 1 1-.966-.26l.237-.882-1.12.646a.5.5 0 0 1-.5-.866l1.12-.646-.884-.237a.5.5 0 1 1 .26-.966l1.848.495.94-.542-.882-.237a.5.5 0 1 1 .258-.966l1.85.495L7 8l-1.236-.713-1.849.49 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string svgSnow {
            get {
                return ResourceManager.GetString("svgSnow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;svg xmlns=&quot;http://www.w3.org/2000/svg&quot; width=&quot;16&quot; height=&quot;16&quot; fill=&quot;currentColor&quot; class=&quot;bi bi-cup-hot-fill&quot; viewBox=&quot;0 0 16 16&quot;&gt;
        ///  &lt;path fill-rule=&quot;evenodd&quot; d=&quot;M.5 6a.5.5 0 0 0-.488.608l1.652 7.434A2.5 2.5 0 0 0 4.104 16h5.792a2.5 2.5 0 0 0 2.44-1.958l.131-.59a3 3 0 0 0 1.3-5.854l.221-.99A.5.5 0 0 0 13.5 6zM13 12.5a2 2 0 0 1-.316-.025l.867-3.898A2.001 2.001 0 0 1 13 12.5&quot;/&gt;
        ///  &lt;path d=&quot;m4.4.8-.003.004-.014.019a4 4 0 0 0-.204.31 2 2 0 0 0-.141.267c-.026.06-.034.092-.037.103v.004a.6.6 0 0 0 .091.248c.075.1 [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string svgWarm {
            get {
                return ResourceManager.GetString("svgWarm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;{{ if .System }}&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;
        ///{{ end }}{{ if .Prompt }}&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///{{ end }}&lt;|im_start|&gt;assistant
        ///{{ .Response }}&lt;|im_end|&gt;
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string yi15Tmp {
            get {
                return ResourceManager.GetString("yi15Tmp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 TEMPLATE &quot;&quot;&quot;&lt;|im_start|&gt;system
        ///{{ .System }}&lt;|im_end|&gt;
        ///&lt;|im_start|&gt;user
        ///{{ .Prompt }}&lt;|im_end|&gt;
        ///&lt;|im_start|&gt;assistant
        ///&quot;&quot;&quot;
        ///PARAMETER stop &quot;&lt;|im_start|&gt;&quot;
        ///PARAMETER stop &quot;&lt;|im_end|&gt;&quot; 的本地化字符串。
        /// </summary>
        internal static string yiTmp {
            get {
                return ResourceManager.GetString("yiTmp", resourceCulture);
            }
        }
    }
}

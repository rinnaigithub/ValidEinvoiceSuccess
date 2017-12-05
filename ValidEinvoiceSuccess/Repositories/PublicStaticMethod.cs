using SMS.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using ValidEinvoiceSuccess.Enums;

namespace ValidEinvoiceSuccess.Repositories
{
    public static class PublicStaticMethod
    {
        public static WorkflowTypeEnum CurrentWorkflowMode = WorkflowTypeEnum.RELEASE;

        /// <summary>
        /// 管理員Email帳號
        /// </summary>
        public static List<string> AdminEmail
        {
            get
            {
                return new List<string>()
                    {
                        "juncheng.liu@rinnai.com.tw"
                    };
            }
        }

        /// <summary>
        /// 管理員群組帳號
        /// </summary>
        public static List<string> AdminEmailGroup
        {
            get
            {
                return new List<string>()
                    {
                        "juncheng.liu@rinnai.com.tw",
                        "Jade.Tai@rinnai.com.tw",
                        "george.chang@rinnai.com.tw"
                    };
            }
        }

        public static string GetDescription(this Enum value)
        {
            // variables
            var enumType = value.GetType();
            var field = enumType.GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // return
            return attributes.Length == 0 ? value.ToString() : ((DescriptionAttribute)attributes[0]).Description;
        }
    }
}
﻿using JeezFoundation.Core.Domain.CodeBuilder.MySQL;
using JeezFoundation.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JeezFoundation.Core.Domain.CodeBuilder
{
    public class StringProvider
    {
        private static StringBuilder Init(string username = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("//------------------------------------------------------------------------------");
            sb.AppendLine("// <auto-generated>");
            sb.AppendLine("//     此代码由工具生成。");
            sb.AppendLine($"//     创建人：{username}");
            sb.AppendLine($"//     创建时间：{DateTime.Now}");
            sb.AppendLine("//     说明：");
            sb.AppendLine("// </auto-generated>");
            sb.AppendLine("//------------------------------------------------------------------------------");
            return sb;
        }
        #region EF

        /// <summary>
        /// 创建MYSQL实体 FOR EF
        /// </summary>
        /// <param name="search"></param>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static StringBuilder GetEFModel(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {search.Namespace}.Models");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public class {table.TABLE_NAME.ToHump()}");
            sb.AppendLine("    {");
            foreach (var item in list)
            {
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {item.COLUMN_COMMENT}");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public {GetType(item)} {item.COLUMN_NAME} {{ get; set; }}");
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            return sb;
        }

        /// <summary>
        /// 获取EF映射
        /// </summary>
        /// <param name="search"></param>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static StringBuilder GetEFMapping(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using JeezFoundation.EntityFramework;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore;");
            sb.AppendLine("using Microsoft.EntityFrameworkCore.Metadata.Builders;");
            sb.AppendLine("using MsSystem.Models.Table;");
            sb.AppendLine();
            sb.AppendLine("namespace MsSystem.Repository.Map.Table.Sys");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT} 映射");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    internal class {table.TABLE_NAME.ToHump()}Map : EntityTypeConfiguration<{table.TABLE_NAME.ToHump()}>");
            sb.AppendLine("    {");
            sb.AppendLine($"        public override void Map(EntityTypeBuilder<{table.TABLE_NAME.ToHump()}> builder)");
            sb.AppendLine("        {");

            foreach (var item in list)
            {
                if (item.COLUMN_KEY == "PRI")
                {
                    sb.AppendLine($"            builder.HasKey(m => m.{item.COLUMN_NAME});");
                    sb.AppendLine();
                    sb.AppendLine($"            builder.ToTable(\"{table.TABLE_NAME}\");");
                    sb.AppendLine();

                    //sb.AppendLine($"            builder.Property(m => m." + item.COLUMN_NAME + ").HasColumnName(\"{item.COLUMN_NAME}\");");
                }
                else
                {
                }
                sb.AppendLine($"            builder.Property(m => m." + item.COLUMN_NAME + ").HasColumnName(\""+ item.COLUMN_NAME + "\");");

            }

            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb;
        }

        public static StringBuilder GetEFIRepository(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);

            sb.AppendLine("using JeezFoundation.EntityFramework.Repositories;");
            sb.AppendLine("using MsSystem.Models.Table.Sys;");
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace MsSystem.IRepository.Table.Sys");
            sb.AppendLine("{");
            var pri = list.First(m => m.COLUMN_KEY == "PRI");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public interface I{table.TABLE_NAME.ToHump()}Repository : IRepositoryEF<{table.TABLE_NAME.ToHump()}, {GetType(pri)}>");
            sb.AppendLine("    {");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            return sb;
        }

        public static StringBuilder GetEFRepository(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using JeezFoundation.Core.Domain.Container;");
            sb.AppendLine("using JeezFoundation.Core.Domain.Uow;");
            sb.AppendLine("using MsSystem.IRepository.Table.Sys;");
            sb.AppendLine("using MsSystem.Models.Table.Sys;");
            sb.AppendLine("using MsSystem.Repository.Context;");
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace MsSystem.Repository.Table.Sys");
            sb.AppendLine("{");

            var pri = list.First(m => m.COLUMN_KEY == "PRI" || m.COLUMN_COMMENT == "主键");

            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public class {table.TABLE_NAME.ToHump()}Repository : Repository<{table.TABLE_NAME.ToHump()}, {GetType(pri)}>, I{table.TABLE_NAME.ToHump()}Repository");
            sb.AppendLine("    {");
            sb.AppendLine($"        public {table.TABLE_NAME.ToHump()}Repository(IUnitOfWork unitOfWork, IStorageContainer container) : base(unitOfWork, container)");
            sb.AppendLine("        {");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb;
        }
        public static StringBuilder GetEFIService(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using JeezFoundation.Core.Domain.Entities;");
            sb.AppendLine("using MsSystem.Models.Table.Sys;");
            sb.AppendLine();
            sb.AppendLine("namespace MsSystem.IService.ISys");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public interface I{table.TABLE_NAME.ToHump()}Service");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            return sb;
        }
        public static StringBuilder GetEFService(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using JeezFoundation.Core.Domain.Entities;");
            sb.AppendLine("using JeezFoundation.Core.Domain.Enum;");
            sb.AppendLine("using JeezFoundation.Core.Linq;");
            sb.AppendLine("using MsSystem.IRepository.Table.Sys;");
            sb.AppendLine("using MsSystem.IService.ISys;");
            sb.AppendLine("using MsSystem.Models.Table.Sys;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine();
            sb.AppendLine("namespace MsSystem.Service.Sys");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public class {table.TABLE_NAME.ToHump()}Service : I{table.TABLE_NAME.ToHump()}Service");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb;
        }
        #endregion

        #region GM-NH

        /// <summary>
        /// 创建MYSQL实体 FOR GM NH
        /// </summary>
        /// <param name="search"></param>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static StringBuilder GetGM_NH_Model(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations; ");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine($"namespace {search.Namespace}.Entity");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public class {table.TABLE_NAME.ToOnlyHump()}");
            sb.AppendLine("    {");
            foreach (var item in list)
            {
                sb.AppendLine("        /// <summary>");
                sb.AppendLine($"        /// {item.COLUMN_COMMENT}");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public {GetType(item)} {item.COLUMN_NAME.ToOnlyHump()} {{ get; set; }}");
                sb.AppendLine();
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            return sb;
        }

        /// <summary>
        /// 获取GM-NH映射
        /// </summary>
        /// <param name="search"></param>
        /// <param name="table"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static StringBuilder GetGM_NH_Mapping(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine($"<hibernate-mapping assembly=\"{search.Namespace}.Entity\" namespace=\"{search.Namespace}.Entity\" xmlns=\"urn:nhibernate-mapping-2.2\">");
            sb.AppendLine($"  <class name=\"{table.TABLE_NAME.ToOnlyHump()}\" table=\"{table.TABLE_NAME.ToLower()}\" lazy=\"true\">");
            foreach (var item in list)
            {
                if (item.COLUMN_KEY == "PRI")
                {
                    sb.AppendLine($"    <id name=\"{item.COLUMN_NAME}\">");
                    sb.AppendLine("      <generator class=\"native\"/>");
                    sb.AppendLine("    </id>");
                }
                else
                {
                    sb.AppendLine($"    <property name=\"{item.COLUMN_NAME}\">");
                    sb.AppendLine($"      <column name=\"{item.COLUMN_NAME}\" sql-type=\"{item.DATA_TYPE}\" not-null=\"{item.IS_NULLABLE == "NO"}\" />");
                    sb.AppendLine("    </property>");
                }
            }
            sb.AppendLine("    </class>");
            sb.AppendLine("</hibernate-mapping>");
            sb.AppendLine();
            return sb;
        }

        public static StringBuilder GetGM_NH_Biz(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using GMJeezFoundationV3.Biz;");
            sb.AppendLine("using GMJeezFoundationV3.Common;");
            sb.AppendLine("using GMJeezFoundationV3.Dal;");
            sb.AppendLine($"using {search.Namespace}.Dal.Dal;");
            sb.AppendLine($"using {search.Namespace}.Entity;");
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine($"namespace {search.Namespace}.Biz.Biz");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public class {table.TABLE_NAME.ToOnlyHump()}Biz : BaseTableBiz<{table.TABLE_NAME.ToOnlyHump()}>");
            sb.AppendLine("    {");
            sb.AppendLine();
            sb.AppendLine($"        private {table.TABLE_NAME.ToOnlyHump()}Dal _dal;");
            sb.AppendLine($"        protected override IRepository<{table.TABLE_NAME.ToOnlyHump()}> Repository");
            sb.AppendLine("        {");
            sb.AppendLine("            get { return _dal; }");

            string priStr = "            set { _dal = ObjectExtensions.As<" + table.TABLE_NAME.ToOnlyHump() + "Dal>(value); }";
            sb.AppendLine(priStr);

            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public void DeleteByKeys(Int32[] ids)");
            sb.AppendLine("        {");
            sb.AppendLine("            base.DeleteByKeys(ids);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb;
        }
        public static StringBuilder GetGM_NH_Dal(TableSearch search, Table table, List<TableColumn> list)
        {
            StringBuilder sb = Init(search.CreateUser);
            sb.AppendLine("using GMJeezFoundationV3.Dal;");
            sb.AppendLine($"using {search.Namespace}.Entity;");
            sb.AppendLine();
            sb.AppendLine($"namespace {search.Namespace}.Dal.Dal");
            sb.AppendLine("{");
            sb.AppendLine("    /// <summary>");
            sb.AppendLine($"    /// {table.TABLE_COMMENT}");
            sb.AppendLine("    /// </summary>");
            sb.AppendLine($"    public class  {table.TABLE_NAME.ToOnlyHump()}Dal:RepositoryTable<{table.TABLE_NAME.ToOnlyHump()}>");
            sb.AppendLine("    {");
            sb.AppendLine("        public override string KeyID");
            sb.AppendLine("        {");
            var pri = list.First(m => m.COLUMN_KEY == "PRI");

            string priStr = "           get { return \"" + pri.COLUMN_NAME + "\"; }";
            sb.AppendLine(priStr);
            sb.AppendLine("           set { base.KeyID = value; } ");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public Oa_ApplyDal()");
            sb.AppendLine("        {");
            sb.AppendLine(string.Format("           base._defaultKeyID= \"{0}\";", pri.COLUMN_NAME));
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine();
            return sb;
        }

        #endregion

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string GetType(TableColumn column)
        {
            string type = "";
            switch (column.DATA_TYPE)
            {
                case "tinyint":
                    type = "byte";
                    break;
                case "int":
                    type = "int";
                    break;
                case "bigint":
                    type = "long";
                    break;
                case "varchar":
                case "text":
                    type = "string";
                    break;
                case "datetime":
                    type = "DateTime";
                    break;
                case "decimal":
                    type = "double";
                    break;
                case "char":
                    type = "Guid";//char规定为GUID类型
                    break;
                default:
                    throw new Exception("存在不支持的类型");
            }
            if (column.IS_NULLABLE == "YES")
            {
                if (type != "string")
                {
                    type += "?";
                }
            }
            return type;
        }

    }

    /// <summary>
    /// 生成代码类型
    /// </summary>
    public enum CodeBuilderType
    {
        EFModel = 1,
        EFMapping = 2,
        EFRepository = 3,
        EFIRepository = 4,
        EFService = 5,
        EFIService = 6,

        GM_NH_Model = 11,
        GM_NH_Mapping = 12,
        GM_NH_Dal = 13,
        GM_NH_Biz = 14
    }
}
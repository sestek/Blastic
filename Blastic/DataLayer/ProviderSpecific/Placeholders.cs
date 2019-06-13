using System.Collections.Generic;

namespace Blastic.DataLayer.ProviderSpecific
{
	public class Placeholders
	{
		public const string IdentityColumnPlaceholder = "C47D08E2-BD94-46AE-94AC-DCA7CB23F10B";
		public const string NVarCharMaxColumnPlaceholder = "245FDEC3-0920-4292-BD5A-66CF730F1ED4";
		public const string BlobColumnPlaceholder = "91EB02F4-776F-45E2-9FD1-F17B46CC7443";
		public const string CaseSensitiveCollationPlaceholder = "97BC9DAE-6D6C-42A6-86A6-96413BE563EA";
		public const string CaseInsensitiveCollationPlaceholder = "E1FB4CC6-D451-46D7-8FCF-80C7C757B538";

		public const string TableExistsKey = "788CE96F-378C-4425-8064-9F9275412C9A";
		public const string TableExistsTableNamePlaceholder = "C0E9F6DC-74F9-4DD1-B5B9-4350B154322F";

		public const string DropIndexKey = "018C9A17-CC21-4E48-B36D-DA1AF02E3AA1";
		public const string DropIndexTableNamePlaceholder = "1F0E3B1E-38CF-4B3E-98E3-7F15670DE195";
		public const string DropIndexIndexNamePlaceholder = "890FC23B-51DB-4FD3-854F-33C0DC0F17B6";

		public const string AlterCollationKey = "5627A1C8-2F6F-4467-BCD8-674183FEB1D8";
		public const string AlterCollationTableNamePlaceholder = "9B16E9E3-3FBC-4D5C-8AEF-7DFBEB3A8353";
		public const string AlterCollationColumnNamePlaceholder = "30E0213E-CFBB-4ACE-ADAE-BB83141F61B3";
		public const string AlterCollationDataTypePlaceholder = "4CE45A04-3929-44E7-A70E-93F1793B7281";

		public const string IgnoreDuplicatesOnIndexPlaceholder = "3C735E5C-41CC-4140-9494-62A91EC3497F";
		public const string IgnoreDuplicatesOnInsertPlaceholder = "3FBC41DF-B3DC-4A60-B5CE-12F1360C6992";

		public const string InsertedRowIdKey = "3435CF01-D4AC-497B-9896-8AE9508385D9";
		public const string InsertedRowIdTableNamePlaceholder = "4C87E20E-CFFA-4E08-8E97-97D2DD2E7F2D";

		public const string PaginationKey = "1A584D22-BB0A-4185-91FA-9F05D18005F0";
		public const string PaginationLimitPlaceholder = "230DBC78-EBFC-4EA1-A2A7-357A5FB3CD9C";
		public const string PaginationOffsetPlaceholder = "F4380078-6B1D-4E5E-B96A-268A2FA6E6DA";

		public static readonly IReadOnlyDictionary<DatabaseProvider, IReadOnlyDictionary<string, string>> ProviderSpecificQueries =
			new Dictionary<DatabaseProvider, IReadOnlyDictionary<string, string>>
		{
			{
				DatabaseProvider.SQLServer, new Dictionary<string, string>
				{
					{ IdentityColumnPlaceholder,           "IDENTITY(1,1)"                                                                         },
					{ NVarCharMaxColumnPlaceholder,        "NVARCHAR(MAX)"                                                                         },
					{ BlobColumnPlaceholder,               "VARBINARY(MAX)"                                                                        },
					{ CaseSensitiveCollationPlaceholder,   "COLLATE Turkish_CS_AS"                                                                 },
					{ CaseInsensitiveCollationPlaceholder, "COLLATE Turkish_CI_AI"                                                                 },
					{ TableExistsKey,                      "SELECT 1 WHERE OBJECT_ID('" + TableExistsTableNamePlaceholder + "', 'U') IS NOT NULL " },
					{ DropIndexKey,                        "DROP INDEX " + DropIndexTableNamePlaceholder + "." + DropIndexIndexNamePlaceholder     },
					{ AlterCollationKey,                   "ALTER TABLE " + AlterCollationTableNamePlaceholder +
					                                       " ALTER COLUMN " + AlterCollationColumnNamePlaceholder +
					                                       " " + AlterCollationDataTypePlaceholder + " COLLATE Turkish_CS_AS"                      },
					{ IgnoreDuplicatesOnIndexPlaceholder,  "WITH IGNORE_DUP_KEY"                                                                   },
					{ IgnoreDuplicatesOnInsertPlaceholder, "INSERT INTO"                                                                           },
					{ InsertedRowIdKey,                    "SELECT SCOPE_IDENTITY()"                                                               },
					{ PaginationKey,                       " OFFSET " + PaginationOffsetPlaceholder + " ROWS " +
					                                       "FETCH NEXT " + PaginationLimitPlaceholder + " ROWS ONLY"                               }
				}
			},
			{
				DatabaseProvider.SQLite, new Dictionary<string, string>
				{
					{ IdentityColumnPlaceholder,           "AUTOINCREMENT"                                      },
					{ NVarCharMaxColumnPlaceholder,        "TEXT"                                               },
					{ BlobColumnPlaceholder,               "BLOB"                                               },
					{ CaseSensitiveCollationPlaceholder,   "COLLATE BINARY"                                     },
					{ CaseInsensitiveCollationPlaceholder, "COLLATE NOCASE"                                     },
					{ TableExistsKey,                      "SELECT 1 FROM sqlite_master WHERE type='table' " +
					                                       "AND name='" + TableExistsTableNamePlaceholder + "'" },
					{ DropIndexKey,                        "DROP INDEX " + DropIndexIndexNamePlaceholder        },
					{ AlterCollationKey,                   "PRAGMA Noop"                                        }, // SQLite does not support altering collation. The default
																											       // collation of SQLite is already case sensitive anyways.
					{ IgnoreDuplicatesOnIndexPlaceholder,  ""                                                   },
					{ IgnoreDuplicatesOnInsertPlaceholder, "INSERT OR IGNORE INTO"                              },
					{ InsertedRowIdKey,                    "SELECT seq FROM sqlite_sequence WHERE name=\"" +
					                                       InsertedRowIdTableNamePlaceholder + "\""             },
					{ PaginationKey,                       " LIMIT " + PaginationLimitPlaceholder +
					                                       " OFFSET " + PaginationOffsetPlaceholder             }
				}
			}
		};
	}
}
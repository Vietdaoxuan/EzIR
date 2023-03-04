2021-08-16 14:01:22.792 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.LoginContext->CheckLogin
            Data    = SPEZIR_CUSTOMER_LOGIN=
	P_USERNAME='test_tungtt',
	P_PASSWORD='',
	P_IP='::1',
	P_BROWSER='Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36',
	P_ROLECODE=test_tungtt,
	P_USERNAME=LOGIN
2021-08-16 14:01:22.975 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = SPEZIR_CUSTOMER_SEARCH=
	P_USERNAME='test_tungtt',
	P_FULLNAME='',
	P_EMAIL='',
	P_ACTIVE='',
	REFCURSOR='',
	P_ROLECODE=LOGIN,
	P_USERNAME=test_tungtt
2021-08-16 14:01:22.975 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=1; 
2021-08-16 14:02:23.049 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.KnowLedgeLevelContext->Select
            Data    = SPEZSTEMP_KNOWLEDGELEVEL_SEARCH=
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:23.050 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.KnowLedgeLevelContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=14; 
2021-08-16 14:02:23.523 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerOrgContext->Select
            Data    = SPEZSTEMP_MANAGERORG_SEARCH=
	P_AMORGID='',
	P_AMORGGROUPID='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:23.523 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerOrgContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=14; 
2021-08-16 14:02:23.602 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = SPEZSTEMP_MANAGER_SEARCH=
	P_AMNGID='',
	P_CPNYID='173',
	P_APPROVE='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:23.602 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=0; 
2021-08-16 14:02:28.900 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Select
            Data    = SPEZSTEMP_INFOSHEET_SEARCH=
	P_ID='',
	P_CPNYID='173',
	P_HASCONTENT='',
	P_MENUID='',
	P_CONTENT='',
	P_TITLE='',
	P_APPROVE='',
	P_POSTDATE='',
	P_LANGUAGE='',
	P_ALEVELID='',
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:28.900 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=0; 
2021-08-16 14:02:40.536 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Insert
            Data    = SPEZSTEMP_INFOSHEET_INSERT=
	P_CPNYID='173',
	P_HASCONTENT='1',
	P_MENUID='635',
	P_CONTENT='<p>test</p>
',
	P_LANGUAGE='VN',
	P_ROLECODE=TTKHDT,
	P_USERNAME=test_tungtt
2021-08-16 14:02:40.586 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='ACONTENT',
	P_VALUE='<p>test</p>
',
	P_STATUS='1',
	P_KEYFUNCTION='Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ - Nguyên liệu đầu vào, Nhà cung cấp',
	P_MENUID='635',
	P_FUNCTION='Thị trường/Khách hàng/Đối thủ',
	P_LEVELID='KT',
	P_DETAILLEVELID='TTKHDT',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:40.594 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Insert
            Data    = SPEZSTEMP_INFOSHEET_INSERT=
	P_CPNYID='173',
	P_HASCONTENT='1',
	P_MENUID='636',
	P_CONTENT='<p>test</p>
',
	P_LANGUAGE='VN',
	P_ROLECODE=TTKHDT,
	P_USERNAME=test_tungtt
2021-08-16 14:02:40.598 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='ACONTENT',
	P_VALUE='<p>test</p>
',
	P_STATUS='1',
	P_KEYFUNCTION='Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ - Cơ cấu doanh thu',
	P_MENUID='636',
	P_FUNCTION='Thị trường/Khách hàng/Đối thủ',
	P_LEVELID='KT',
	P_DETAILLEVELID='TTKHDT',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:40.608 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Insert
            Data    = SPEZSTEMP_INFOSHEET_INSERT=
	P_CPNYID='173',
	P_HASCONTENT='1',
	P_MENUID='637',
	P_CONTENT='<p>test</p>
',
	P_LANGUAGE='VN',
	P_ROLECODE=TTKHDT,
	P_USERNAME=test_tungtt
2021-08-16 14:02:40.611 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='ACONTENT',
	P_VALUE='<p>test</p>
',
	P_STATUS='1',
	P_KEYFUNCTION='Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ - Sản phẩm thay thế',
	P_MENUID='637',
	P_FUNCTION='Thị trường/Khách hàng/Đối thủ',
	P_LEVELID='KT',
	P_DETAILLEVELID='TTKHDT',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:40.614 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Insert
            Data    = SPEZSTEMP_INFOSHEET_INSERT=
	P_CPNYID='173',
	P_HASCONTENT='1',
	P_MENUID='638',
	P_CONTENT='<p>test</p>
',
	P_LANGUAGE='VN',
	P_ROLECODE=TTKHDT,
	P_USERNAME=test_tungtt
2021-08-16 14:02:40.621 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='ACONTENT',
	P_VALUE='<p>test</p>
',
	P_STATUS='1',
	P_KEYFUNCTION='Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ - Đối thủ cạnh tranh',
	P_MENUID='638',
	P_FUNCTION='Thị trường/Khách hàng/Đối thủ',
	P_LEVELID='KT',
	P_DETAILLEVELID='TTKHDT',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:40.639 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Insert
            Data    = SPEZSTEMP_INFOSHEET_INSERT=
	P_CPNYID='173',
	P_HASCONTENT='1',
	P_MENUID='639',
	P_CONTENT='<p>test</p>
',
	P_LANGUAGE='VN',
	P_ROLECODE=TTKHDT,
	P_USERNAME=test_tungtt
2021-08-16 14:02:40.662 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='ACONTENT',
	P_VALUE='<p>test</p>
',
	P_STATUS='1',
	P_KEYFUNCTION='Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ - Thị trường khách hàng',
	P_MENUID='639',
	P_FUNCTION='Thị trường/Khách hàng/Đối thủ',
	P_LEVELID='KT',
	P_DETAILLEVELID='TTKHDT',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:40.668 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Insert
            Data    = SPEZSTEMP_INFOSHEET_INSERT=
	P_CPNYID='173',
	P_HASCONTENT='1',
	P_MENUID='651',
	P_CONTENT='<p>test</p>
',
	P_LANGUAGE='VN',
	P_ROLECODE=TTKHDT,
	P_USERNAME=test_tungtt
2021-08-16 14:02:40.671 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='ACONTENT',
	P_VALUE='<p>test</p>
',
	P_STATUS='1',
	P_KEYFUNCTION='Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ - Thị trường khách hàng đối thủ khác',
	P_MENUID='651',
	P_FUNCTION='Thị trường/Khách hàng/Đối thủ',
	P_LEVELID='KT',
	P_DETAILLEVELID='TTKHDT',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:41.653 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Select
            Data    = SPEZSTEMP_INFOSHEET_SEARCH=
	P_ID='',
	P_CPNYID='173',
	P_HASCONTENT='',
	P_MENUID='',
	P_CONTENT='',
	P_TITLE='',
	P_APPROVE='',
	P_POSTDATE='',
	P_LANGUAGE='',
	P_ALEVELID='',
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:02:41.654 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.InfoSheetContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=43; 
2021-08-16 14:03:42.512 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:42.512 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:42.851 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:42.851 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.001 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = SPEZSTEMP_SUBCOMPANY_SEARCH=
	P_CPNYID='173',
	REFCURSORSUBSIDIARIES='',
	REFCURSORPARTNER='',
	REFCURSORSHAREHOLDER='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.001 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = dataSet.Tables.Count=3; dataTable[0].Rows.Count=2; dataTable[1].Rows.Count=1; dataTable[2].Rows.Count=50; 
2021-08-16 14:03:43.071 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.071 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.084 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.084 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.095 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.095 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.105 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.105 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.123 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.123 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.127 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.127 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.158 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.158 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.169 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.169 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.191 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.191 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.194 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.194 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.203 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.203 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:03:43.260 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.260 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.309 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.309 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.372 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.373 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.439 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.440 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.492 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.493 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.603 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.604 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.639 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.639 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.692 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.692 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:43.726 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:43.726 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:03:56.865 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.KnowLedgeLevelContext->Select
            Data    = SPEZSTEMP_KNOWLEDGELEVEL_SEARCH=
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:56.865 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.KnowLedgeLevelContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=14; 
2021-08-16 14:03:57.144 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerOrgContext->Select
            Data    = SPEZSTEMP_MANAGERORG_SEARCH=
	P_AMORGID='',
	P_AMORGGROUPID='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:57.144 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = SPEZSTEMP_MANAGER_SEARCH=
	P_AMNGID='',
	P_CPNYID='173',
	P_APPROVE='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:03:57.144 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=0; 
2021-08-16 14:03:57.144 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerOrgContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=14; 
2021-08-16 14:04:02.219 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.219 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.276 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.277 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.417 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = SPEZSTEMP_SUBCOMPANY_SEARCH=
	P_CPNYID='173',
	REFCURSORSUBSIDIARIES='',
	REFCURSORPARTNER='',
	REFCURSORSHAREHOLDER='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.417 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = dataSet.Tables.Count=3; dataTable[0].Rows.Count=2; dataTable[1].Rows.Count=1; dataTable[2].Rows.Count=50; 
2021-08-16 14:04:02.451 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.451 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.470 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.470 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.482 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.482 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.482 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.483 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.501 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.501 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.517 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.517 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.528 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.528 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.537 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.537 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.540 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.540 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.554 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.554 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:02.575 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.575 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.646 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.646 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.687 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.687 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.716 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.716 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.748 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.748 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.790 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.790 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.838 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.838 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.875 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.875 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.920 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.920 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:02.954 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:02.955 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:04.176 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:04.176 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:04.205 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:04.206 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:17.381 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->InsertSubcompany
            Data    = SPEZSTEMP_SUBCOMPANY_INSERT=
	P_ASUBCOMPANYNAME='congtycon_6',
	P_ADDRESS='HN',
	P_MINISTRY='3',
	P_ASHARERATE='25.0',
	P_CPNYID='173',
	P_ASUBCOMPANYID='0',
	P_ASUBCOMPANYTYPEID='1',
	P_AAPPROVE='1',
	P_ANOTE='asubcompanyname;aaddress;asharerate;aministryid;asubcompanytypeid;',
	P_ROLECODE=CCSH,
	P_USERNAME=test_tungtt
2021-08-16 14:04:17.625 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='asubcompanyname',
	P_VALUE='congtycon_6',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Tên',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:17.685 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='aaddress',
	P_VALUE='HN',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Địa chỉ',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:17.714 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='asharerate',
	P_VALUE='25.0',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Nắm giữ',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:17.761 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='aministryid',
	P_VALUE='3',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Thuộc ngành',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:17.797 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='173',
	P_KEY='asubcompanytypeid',
	P_VALUE='1',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Loại hình công ty con',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.103 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = SPEZSTEMP_SUBCOMPANY_SEARCH=
	P_CPNYID='173',
	REFCURSORSUBSIDIARIES='',
	REFCURSORPARTNER='',
	REFCURSORSHAREHOLDER='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.104 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = dataSet.Tables.Count=3; dataTable[0].Rows.Count=3; dataTable[1].Rows.Count=1; dataTable[2].Rows.Count=50; 
2021-08-16 14:04:18.170 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.170 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.200 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.200 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.206 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.206 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.231 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.231 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.247 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.247 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.258 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.258 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.273 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.273 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.274 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.274 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.286 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.286 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.299 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.300 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.309 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.309 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.313 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.313 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.333 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.333 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.344 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.344 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.349 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.350 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.360 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.360 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.371 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.371 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.377 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.377 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.382 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.382 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:04:18.425 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.425 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.456 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.456 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.504 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.504 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.556 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.557 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.626 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.626 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.669 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.669 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.718 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.718 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.749 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.749 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.784 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.784 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.814 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.814 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.847 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.847 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.885 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.885 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.918 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.919 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:04:18.958 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:04:18.958 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:05:32.468 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.LoginContext->CheckLogin
            Data    = SPEZIR_CUSTOMER_LOGIN=
	P_USERNAME='tungtt_2',
	P_PASSWORD='',
	P_IP='::1',
	P_BROWSER='Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36',
	P_ROLECODE=tungtt_2,
	P_USERNAME=LOGIN
2021-08-16 14:05:32.481 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = SPEZIR_CUSTOMER_SEARCH=
	P_USERNAME='tungtt_2',
	P_FULLNAME='',
	P_EMAIL='',
	P_ACTIVE='',
	REFCURSOR='',
	P_ROLECODE=LOGIN,
	P_USERNAME=tungtt_2
2021-08-16 14:05:32.481 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=1; 
2021-08-16 14:05:35.290 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:35.290 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:05:35.317 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:35.317 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:05:35.353 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = SPEZSTEMP_SUBCOMPANY_SEARCH=
	P_CPNYID='1595',
	REFCURSORSUBSIDIARIES='',
	REFCURSORPARTNER='',
	REFCURSORSHAREHOLDER='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:35.353 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = dataSet.Tables.Count=3; dataTable[0].Rows.Count=0; dataTable[1].Rows.Count=0; dataTable[2].Rows.Count=0; 
2021-08-16 14:05:36.894 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:36.894 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:05:36.918 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:36.918 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:05:44.947 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->InsertSubcompany
            Data    = SPEZSTEMP_SUBCOMPANY_INSERT=
	P_ASUBCOMPANYNAME='abc',
	P_ADDRESS='HN',
	P_MINISTRY='4',
	P_ASHARERATE='22.0',
	P_CPNYID='1595',
	P_ASUBCOMPANYID='0',
	P_ASUBCOMPANYTYPEID='3',
	P_AAPPROVE='1',
	P_ANOTE='asubcompanyname;aaddress;asharerate;aministryid;asubcompanytypeid;',
	P_ROLECODE=CCSH,
	P_USERNAME=tungtt_2
2021-08-16 14:05:45.072 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='1595',
	P_KEY='asubcompanyname',
	P_VALUE='abc',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Tên',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.088 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='1595',
	P_KEY='aaddress',
	P_VALUE='HN',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Địa chỉ',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.100 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='1595',
	P_KEY='asharerate',
	P_VALUE='22.0',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Nắm giữ',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.136 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='1595',
	P_KEY='aministryid',
	P_VALUE='4',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Thuộc ngành',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.151 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='1595',
	P_KEY='asubcompanytypeid',
	P_VALUE='3',
	P_STATUS='2',
	P_KEYFUNCTION='Công ty con, liên kết - Loại hình công ty con',
	P_MENUID='677',
	P_FUNCTION='Cơ cấu sở hữu',
	P_LEVELID='TQ',
	P_DETAILLEVELID='CCSH',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.394 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = SPEZSTEMP_SUBCOMPANY_SEARCH=
	P_CPNYID='1595',
	REFCURSORSUBSIDIARIES='',
	REFCURSORPARTNER='',
	REFCURSORSHAREHOLDER='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.394 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select
            Data    = dataSet.Tables.Count=3; dataTable[0].Rows.Count=1; dataTable[1].Rows.Count=0; dataTable[2].Rows.Count=0; 
2021-08-16 14:05:45.430 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.430 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:05:45.441 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.441 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:05:45.456 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.456 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:05:45.472 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.472 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:05:45.496 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = SPEZSTEMP_PAR_SUBCOMPANYTYPE_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.496 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_SubCompanyType
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 
2021-08-16 14:05:45.515 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.515 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:05:45.554 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.554 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:05:45.587 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = SPEZSTEMP_MINISTRY_SEARCH=
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:05:45.587 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CoCauSoHuuContext->Select_Misnitry
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=91; 
2021-08-16 14:13:36.760 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CommonTypeContext->Select
            Data    = SPEZIR_COMMONTYPE_GET_BY_CATEGORY=
	P_ACATEGORY='1,3,4,11',
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:13:36.761 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CommonTypeContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=38; 
2021-08-16 14:13:37.551 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CommonTypeContext->Select
            Data    = SPEZIR_COMMONTYPE_GET_BY_CATEGORY=
	P_ACATEGORY='1,3,4,11',
	REFCURSOR='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-16 14:13:37.551 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CommonTypeContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=38; 
2021-08-16 14:13:37.887 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CongBoThongTinContext->Select
            Data    = Spezir_News_Search=
	P_ANewID='',
	P_ADocType='',
	P_ANewType='',
	P_FROMDATE='',
	P_TODATE='',
	P_ACreateBy='',
	P_ACpnyID='1595',
	P_Username='tungtt_2',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=tungtt_2
2021-08-16 14:13:37.887 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CongBoThongTinContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=0; 

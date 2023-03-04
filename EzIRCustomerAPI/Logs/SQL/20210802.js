2021-08-02 10:44:46.839 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.LoginContext->CheckLogin
            Data    = SPEZIR_CUSTOMER_LOGIN=
	P_USERNAME='Admin',
	P_PASSWORD='',
	P_ROLECODE=Admin,
	P_USERNAME=LOGIN
2021-08-02 10:44:47.142 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = SPEZIR_CUSTOMER_SEARCH=
	P_USERNAME='Admin',
	P_FULLNAME='',
	P_EMAIL='',
	P_ACTIVE='',
	REFCURSOR='',
	P_ROLECODE=LOGIN,
	P_USERNAME=Admin
2021-08-02 10:44:47.143 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=1; 
2021-08-02 13:18:40.124 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.LoginContext->CheckLogin
            Data    = SPEZIR_CUSTOMER_LOGIN=
	P_USERNAME='Admin',
	P_PASSWORD='',
	P_ROLECODE=Admin,
	P_USERNAME=LOGIN
2021-08-02 13:18:40.234 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = SPEZIR_CUSTOMER_SEARCH=
	P_USERNAME='Admin',
	P_FULLNAME='',
	P_EMAIL='',
	P_ACTIVE='',
	REFCURSOR='',
	P_ROLECODE=LOGIN,
	P_USERNAME=Admin
2021-08-02 13:18:40.235 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.CustomerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=1; 
2021-08-02 13:18:45.409 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.KnowLedgeLevelContext->Select
            Data    = SPEZSTEMP_KNOWLEDGELEVEL_SEARCH=
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:45.409 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.KnowLedgeLevelContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=14; 
2021-08-02 13:18:45.747 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerOrgContext->Select
            Data    = SPEZSTEMP_MANAGERORG_SEARCH=
	P_AMORGID='',
	P_AMORGGROUPID='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:45.747 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerOrgContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=14; 
2021-08-02 13:18:46.066 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = SPEZSTEMP_MANAGER_SEARCH=
	P_AMNGID='',
	P_CPNYID='341',
	P_APPROVE='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:46.067 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=2; 
2021-08-02 13:18:57.183 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Insert
            Data    = SPEZSTEMP_MANAGER_INSERT=
	P_CPNYID='341',
	P_ACORGID='',
	P_AMNGERNAME='test_2',
	P_ANATIONALITY='',
	P_AKNOWLEDGELEVELID='1',
	P_AISLEGALREP='0',
	P_ADATEOFBIRTHVN='',
	P_AKNOWLEDGESPECIALLEVEL='',
	P_AKNOWLEDGESPECIALLEVELEN='',
	P_ACVPATH='',
	P_AORD='',
	P_ANOTE='',
	P_MORGID='3',
	P_ROLECODE=TPLD,
	P_USERNAME=Admin
2021-08-02 13:18:57.227 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='341',
	P_KEY='MNGERNAME',
	P_VALUE='test_2',
	P_STATUS='1',
	P_KEYFUNCTION='Thành phần lãnh đạo - MNGERNAME',
	P_MENUID='581',
	P_FUNCTION='Thành phần lãnh đạo',
	P_LEVELID='TQ',
	P_DETAILLEVELID='TPLD',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:57.258 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='341',
	P_KEY='KNOWLEDGELEVELID',
	P_VALUE='Trên ĐH',
	P_STATUS='1',
	P_KEYFUNCTION='Thành phần lãnh đạo - KNOWLEDGELEVELID',
	P_MENUID='581',
	P_FUNCTION='Thành phần lãnh đạo',
	P_LEVELID='TQ',
	P_DETAILLEVELID='TPLD',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:57.265 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='341',
	P_KEY='ISLEGALREP',
	P_VALUE='False',
	P_STATUS='1',
	P_KEYFUNCTION='Thành phần lãnh đạo - ISLEGALREP',
	P_MENUID='581',
	P_FUNCTION='Thành phần lãnh đạo',
	P_LEVELID='TQ',
	P_DETAILLEVELID='TPLD',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:57.275 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ChangeInfoContext->Insert
            Data    = SPEZSTEMP_CHANGEINFO_INSERT=
	P_CPNYID='341',
	P_KEY='MORGID',
	P_VALUE='["Thành viên HĐQT"]',
	P_STATUS='1',
	P_KEYFUNCTION='Thành phần lãnh đạo - MORGID',
	P_MENUID='581',
	P_FUNCTION='Thành phần lãnh đạo',
	P_LEVELID='TQ',
	P_DETAILLEVELID='TPLD',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:57.767 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = SPEZSTEMP_MANAGER_SEARCH=
	P_AMNGID='',
	P_CPNYID='341',
	P_APPROVE='',
	Refcursor='',
	P_ROLECODE=,
	P_USERNAME=
2021-08-02 13:18:57.767 +07:00 [INF] =================
            Source  = EzIRCustomerAPI.DataAccess.Implementations.ManagerContext->Select
            Data    = dataSet.Tables.Count=1; dataTable[0].Rows.Count=3; 

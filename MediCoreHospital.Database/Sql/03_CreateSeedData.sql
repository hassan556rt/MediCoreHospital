INSERT INTO Roles (RoleName, Description) VALUES
('Super Admin', 'إدارة كاملة للنظام'),
('Administrator', 'إدارة العمليات الإدارية'),
('Doctor', 'طبيب'),
('Nurse', 'ممرض/ة'),
('Receptionist', 'إستقبال'),
('Pharmacist', 'صيدلي'),
('Laboratory Technician', 'فني مختبر'),
('Radiology Technician', 'فني أشعة'),
('Accountant', 'محاسب'),
('HR', 'الموارد البشرية'),
('Inventory Manager', 'مدير المخزون');

INSERT INTO Permissions (PermissionName, Description) VALUES
('Patients.View', 'عرض المرضى'),
('Patients.Create', 'إضافة المرضى'),
('Patients.Edit', 'تعديل المرضى'),
('Appointments.View', 'عرض المواعيد'),
('Appointments.Create', 'إنشاء المواعيد'),
('Billing.View', 'عرض الفواتير'),
('Billing.Create', 'إنشاء الفواتير'),
('Reports.View', 'عرض التقارير'),
('Users.Manage', 'إدارة المستخدمين'),
('System.Settings', 'إعدادات النظام');

INSERT INTO Departments (DepartmentName, Description) VALUES
('Reception', 'الإستقبال'),
('Emergency', 'الطوارئ'),
('Internal Medicine', 'الطب الباطني'),
('Pediatrics', 'الأطفال'),
('Radiology', 'الأشعة'),
('Laboratory', 'المختبر'),
('Pharmacy', 'الصيدلة'),
('Nursing', 'التمريض');

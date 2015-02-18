Name: stm
Version: 0.4
Release: 1
Group: Applications/Other
Source: %{name}-%{version}.tar.gz
BuildArch: noarch
Summary: Simple Server Manager
URL: https://github.com/xvitaly/stm
License: GPLv3
BuildRoot: %{_tmppath}/%{name}-root
Vendor: EasyCoding Team
Requires: mono-core

%description
Simple Server Manager can be used to generate, reset or view tokens for Steam game servers. This tool is completely free and open-source.

%prep
%setup -q -c -n %{name}

%build
# Do nothing...

%install
# Executting cleanup...
rm -rf %{buildroot}

# Creating base directories...
mkdir -p %{buildroot}/usr/bin
mkdir -p %{buildroot}/usr/share/%{name}

# Copying files...
cp -fpr %_builddir/* %{buildroot}/usr/share

# Creating launcher...
echo "#!/bin/sh" > %{buildroot}/usr/bin/%{name}
echo "/usr/bin/mono \"/usr/share/%{name}/%{name}.exe\" \"\$@\"" >> %{buildroot}/usr/bin/%{name}

# Making our script executtable...
chmod +x %{buildroot}/usr/bin/%{name}

# Generating list of files...
find %{buildroot} -not -type d -printf "/%%P\n" | sed '/\/man\//s/$/\*/' > manifest

%files -f manifest
%defattr(-,root,root)

%changelog
* Wed Jan 18 2015 V1TSK <vitaly@easycoding.org>
- Updated SPEC file. Changed Arch to noarch.

* Mon Jan 16 2015 V1TSK <vitaly@easycoding.org>
- First RPM release for Fedora/openSUSE.

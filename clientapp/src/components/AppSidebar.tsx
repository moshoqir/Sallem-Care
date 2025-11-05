import { Home, LogOut, Activity, Stethoscope } from "lucide-react";
import { NavLink, useNavigate } from "react-router-dom";
import {
  Sidebar,
  SidebarContent,
  SidebarGroup,
  SidebarGroupContent,
  SidebarGroupLabel,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarHeader,
  SidebarFooter,
  useSidebar,
} from "@/components/ui/sidebar";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/context/AuthContext";
import { Heart } from "lucide-react";

const doctorItems = [
  { title: "Dashboard", url: "/dashboard", icon: Home },
  { title: "Medical Dashboard", url: "/dashboard/medical", icon: Stethoscope },
  { title: "Medical Analysis", url: "/dashboard/analysis", icon: Activity },
];

const nurseItems = [
  { title: "Dashboard", url: "/dashboard", icon: Home },
  { title: "Medical Dashboard", url: "/dashboard/medical", icon: Stethoscope },
  { title: "Medical Analysis", url: "/dashboard/analysis", icon: Activity },
];

const adminItems = [
  { title: "Dashboard", url: "/dashboard", icon: Home },
  { title: "Medical Dashboard", url: "/dashboard/medical", icon: Stethoscope },
  { title: "Medical Analysis", url: "/dashboard/analysis", icon: Activity },
];

export function AppSidebar() {
  const { state } = useSidebar();
  const { userRole, signOut } = useAuth();
  const navigate = useNavigate();
  const collapsed = state === "collapsed";

  const getMenuItems = () => {
    switch (userRole) {
      case "doctor":
        return doctorItems;
      case "nurse":
        return nurseItems;
      case "admin":
        return adminItems;
      default:
        return [];
    }
  };

  const menuItems = getMenuItems();

  const handleSignOut = async () => {
    await signOut();
  };

  return (
    <Sidebar className={collapsed ? "w-14" : "w-64"} collapsible="icon">
      <SidebarHeader className="border-b border-sidebar-border p-4">
        <div className="flex items-center gap-2">
          <div className="flex items-center justify-center w-10 h-10 rounded-lg bg-gradient-to-br from-primary to-secondary">
            <Heart className="w-5 h-5 text-white" />
          </div>
          {!collapsed && (
            <div>
              <h2 className="text-sm font-semibold text-sidebar-foreground">Saleem Care</h2>
              <p className="text-xs text-sidebar-foreground/70 capitalize">{userRole}</p>
            </div>
          )}
        </div>
      </SidebarHeader>

      <SidebarContent>
        <SidebarGroup>
          <SidebarGroupLabel>Navigation</SidebarGroupLabel>
          <SidebarGroupContent>
            <SidebarMenu>
              {menuItems.map((item) => (
                <SidebarMenuItem key={item.title}>
                  <SidebarMenuButton asChild>
                    <NavLink
                      to={item.url}
                      end={item.url === "/dashboard"}
                      className={({ isActive }) =>
                        isActive
                          ? "bg-sidebar-accent text-sidebar-accent-foreground font-medium"
                          : "hover:bg-sidebar-accent/50"
                      }
                    >
                      <item.icon className="h-4 w-4" />
                      {!collapsed && <span>{item.title}</span>}
                    </NavLink>
                  </SidebarMenuButton>
                </SidebarMenuItem>
              ))}
            </SidebarMenu>
          </SidebarGroupContent>
        </SidebarGroup>
      </SidebarContent>

      <SidebarFooter className="border-t border-sidebar-border p-4">
        <Button
          variant="ghost"
          className="w-full justify-start text-sidebar-foreground hover:bg-sidebar-accent/50"
          onClick={handleSignOut}
        >
          <LogOut className="h-4 w-4" />
          {!collapsed && <span className="ml-2">Sign Out</span>}
        </Button>
      </SidebarFooter>
    </Sidebar>
  );
}

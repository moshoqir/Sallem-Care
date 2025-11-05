import { useAuth } from "@/context/AuthContext";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Activity, Users, Calendar, FileText, TrendingUp, Clock } from "lucide-react";

export default function Dashboard() {
  const { user, userRole } = useAuth();

  const getDoctorStats = () => [
    { title: "Today's Appointments", value: "12", icon: Calendar, color: "text-primary" },
    { title: "Active Patients", value: "48", icon: Users, color: "text-secondary" },
    { title: "Pending Reports", value: "5", icon: FileText, color: "text-accent" },
    { title: "Consultations", value: "8", icon: Activity, color: "text-primary" },
  ];

  const getNurseStats = () => [
    { title: "Patients in Care", value: "24", icon: Users, color: "text-primary" },
    { title: "Scheduled Tasks", value: "15", icon: Clock, color: "text-secondary" },
    { title: "Vital Checks", value: "32", icon: Activity, color: "text-accent" },
    { title: "Medication Rounds", value: "6", icon: Calendar, color: "text-primary" },
  ];

  const getAdminStats = () => [
    { title: "Total Staff", value: "124", icon: Users, color: "text-primary" },
    { title: "Departments", value: "8", icon: TrendingUp, color: "text-secondary" },
    { title: "Active Cases", value: "89", icon: Activity, color: "text-accent" },
    { title: "Reports Due", value: "12", icon: FileText, color: "text-primary" },
  ];

  const getStats = () => {
    switch (userRole) {
      case "doctor":
        return getDoctorStats();
      case "nurse":
        return getNurseStats();
      case "admin":
        return getAdminStats();
      default:
        return [];
    }
  };

  const stats = getStats();

  const getWelcomeMessage = () => {
    const hour = new Date().getHours();
    const greeting = hour < 12 ? "Good morning" : hour < 18 ? "Good afternoon" : "Good evening";
    const roleTitle = userRole === "admin" ? "Manager" : userRole?.charAt(0).toUpperCase() + userRole?.slice(1);
    return `${greeting}, ${roleTitle}`;
  };

  return (
    <div className="space-y-8">
      <div>
        <h1 className="text-3xl font-bold text-foreground">{getWelcomeMessage()}</h1>
        <p className="text-muted-foreground mt-2">Here's what's happening today</p>
      </div>

      <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-4">
        {stats.map((stat, index) => (
          <Card key={stat.title} className="animate-fade-in shadow-card hover:shadow-elevated transition-shadow" style={{ animationDelay: `${index * 100}ms` }}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                {stat.title}
              </CardTitle>
              <stat.icon className={`h-5 w-5 ${stat.color}`} />
            </CardHeader>
            <CardContent>
              <div className="text-3xl font-bold text-foreground">{stat.value}</div>
            </CardContent>
          </Card>
        ))}
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card className="shadow-card">
          <CardHeader>
            <CardTitle>Recent Activity</CardTitle>
            <CardDescription>Your latest updates and notifications</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {[1, 2, 3].map((item) => (
                <div key={item} className="flex items-start gap-4 p-3 rounded-lg bg-muted/50">
                  <div className="w-2 h-2 rounded-full bg-primary mt-2" />
                  <div>
                    <p className="text-sm font-medium">Sample activity #{item}</p>
                    <p className="text-xs text-muted-foreground">{item} hour(s) ago</p>
                  </div>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>

        <Card className="shadow-card">
          <CardHeader>
            <CardTitle>Quick Actions</CardTitle>
            <CardDescription>Frequently used features</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="grid grid-cols-2 gap-3">
              {["Action 1", "Action 2", "Action 3", "Action 4"].map((action) => (
                <button
                  key={action}
                  className="p-4 rounded-lg border border-border hover:bg-muted/50 transition-colors text-sm font-medium"
                >
                  {action}
                </button>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}

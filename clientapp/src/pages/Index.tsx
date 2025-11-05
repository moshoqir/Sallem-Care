import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/context/AuthContext";
import { Button } from "@/components/ui/button";
import { Heart, Activity, Users, Shield } from "lucide-react";

const Index = () => {
  const navigate = useNavigate();
  const { user } = useAuth();

  useEffect(() => {
    // Check if user is already logged in
    if (user) {
      navigate("/dashboard");
    }
  }, [user, navigate]);

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary/5 via-background to-secondary/5">
      <div className="container mx-auto px-4 py-16">
        <div className="text-center mb-16 animate-fade-in">
          <div className="inline-flex items-center justify-center w-20 h-20 rounded-full bg-gradient-to-br from-primary to-secondary mb-6 shadow-xl">
            <Heart className="w-10 h-10 text-white" />
          </div>
          <h1 className="text-5xl md:text-6xl font-bold mb-4 bg-gradient-to-r from-primary to-secondary bg-clip-text text-transparent">
            Saleem Care System
          </h1>
          <p className="text-xl text-muted-foreground max-w-2xl mx-auto">
            Intelligent Healthcare Management for Modern Medical Professionals
          </p>
        </div>

        <div className="grid md:grid-cols-3 gap-8 max-w-5xl mx-auto mb-16">
          <div className="bg-card p-8 rounded-xl shadow-card hover:shadow-elevated transition-shadow animate-scale-in" style={{ animationDelay: "100ms" }}>
            <div className="w-12 h-12 rounded-lg bg-primary/10 flex items-center justify-center mb-4">
              <Activity className="w-6 h-6 text-primary" />
            </div>
            <h3 className="text-xl font-semibold mb-2">For Doctors</h3>
            <p className="text-muted-foreground">
              Manage patients, appointments, and medical records efficiently
            </p>
          </div>

          <div className="bg-card p-8 rounded-xl shadow-card hover:shadow-elevated transition-shadow animate-scale-in" style={{ animationDelay: "200ms" }}>
            <div className="w-12 h-12 rounded-lg bg-secondary/10 flex items-center justify-center mb-4">
              <Users className="w-6 h-6 text-secondary" />
            </div>
            <h3 className="text-xl font-semibold mb-2">For Nurses</h3>
            <p className="text-muted-foreground">
              Streamline patient care tasks and daily schedules
            </p>
          </div>

          <div className="bg-card p-8 rounded-xl shadow-card hover:shadow-elevated transition-shadow animate-scale-in" style={{ animationDelay: "300ms" }}>
            <div className="w-12 h-12 rounded-lg bg-accent/10 flex items-center justify-center mb-4">
              <Shield className="w-6 h-6 text-accent" />
            </div>
            <h3 className="text-xl font-semibold mb-2">For Managers</h3>
            <p className="text-muted-foreground">
              Comprehensive oversight of staff and hospital operations
            </p>
          </div>
        </div>

        <div className="text-center animate-fade-in" style={{ animationDelay: "400ms" }}>
          <Button
            size="lg"
            className="text-lg px-8 py-6 shadow-lg hover:shadow-xl transition-all"
            onClick={() => navigate("/auth")}
          >
            Get Started
          </Button>
        </div>
      </div>
    </div>
  );
};

export default Index;

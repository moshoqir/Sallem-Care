import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { User, MapPin } from "lucide-react";

export const BodyModel = () => {
  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center">
          <User className="w-5 h-5 mr-2 text-primary" />
          Pain Location
        </CardTitle>
      </CardHeader>
      <CardContent>
        <div className="relative mx-auto max-w-xs">
          {/* SVG Human Body Model */}
          <svg
            viewBox="0 0 200 400"
            className="w-full h-auto"
            xmlns="http://www.w3.org/2000/svg"
          >
            {/* Head */}
            <ellipse cx="100" cy="30" rx="20" ry="25" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            
            {/* Torso */}
            <rect x="75" y="55" width="50" height="80" rx="10" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            
            {/* Arms */}
            <rect x="45" y="65" width="15" height="60" rx="7" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            <rect x="140" y="65" width="15" height="60" rx="7" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            
            {/* Abdomen - Normal */}
            <rect x="80" y="135" width="40" height="40" rx="5" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            
            {/* Lower Right Quadrant - Pain Area (Highlighted) */}
            <rect x="100" y="150" width="20" height="25" rx="3" fill="hsl(var(--warning))" stroke="hsl(var(--destructive))" strokeWidth="2" className="animate-pulse"/>
            
            {/* Pelvis */}
            <rect x="85" y="175" width="30" height="25" rx="5" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            
            {/* Legs */}
            <rect x="85" y="200" width="12" height="80" rx="6" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            <rect x="103" y="200" width="12" height="80" rx="6" fill="hsl(var(--muted))" stroke="hsl(var(--border))" strokeWidth="1"/>
            
            {/* Pain Point Marker */}
            <circle cx="110" cy="162" r="3" fill="hsl(var(--destructive))" className="animate-ping"/>
            <circle cx="110" cy="162" r="3" fill="hsl(var(--destructive))"/>
          </svg>
          
          {/* Pain Location Label */}
          <div className="absolute top-1/2 right-0 transform translate-x-full -translate-y-1/2">
            <div className="bg-destructive text-destructive-foreground px-2 py-1 rounded text-xs whitespace-nowrap">
              <MapPin className="w-3 h-3 inline mr-1" />
              RLQ Pain
            </div>
          </div>
        </div>
        
        <div className="mt-4 space-y-3">
          <div className="flex items-center justify-between">
            <span className="text-sm text-foreground">Pain Intensity</span>
            <Badge variant="outline" className="bg-destructive-light text-destructive border-destructive/20">
              8/10 - Severe
            </Badge>
          </div>
          
          <div className="flex items-center justify-between">
            <span className="text-sm text-foreground">Pain Type</span>
            <span className="text-sm text-muted-foreground">Sharp, Constant</span>
          </div>
          
          <div className="flex items-center justify-between">
            <span className="text-sm text-foreground">Radiation</span>
            <span className="text-sm text-muted-foreground">None</span>
          </div>
          
          <div className="flex items-center justify-between">
            <span className="text-sm text-foreground">Aggravating Factors</span>
            <span className="text-sm text-muted-foreground">Movement, Coughing</span>
          </div>
        </div>
        
        <div className="mt-4 p-3 bg-warning-light rounded-lg">
          <p className="text-sm text-warning">
            <strong>McBurney's Point:</strong> Maximum tenderness located at the junction of the outer 1/3 and inner 2/3 of the line from anterior superior iliac spine to umbilicus.
          </p>
        </div>
      </CardContent>
    </Card>
  );
};
import { PatientHeader } from "@/components/medical/PatientHeader";
import { DiagnosticResults } from "@/components/medical/DiagnosticResults";
import { SymptomsAnalysis } from "@/components/medical/SymptomsAnalysis";
import { VitalSigns } from "@/components/medical/VitalSigns";
import { BodyModel } from "@/components/medical/BodyModel";
import { MedicalRecommendations } from "@/components/medical/MedicalRecommendations";
import { PhysicianNotes } from "@/components/medical/PhysicianNotes";
import { SeverityIndicator } from "@/components/medical/SeverityIndicator";

const AnalysisMedical = () => {
  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <header className="bg-card border-b border-border shadow-sm">
        <div className="max-w-7xl mx-auto px-6 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center space-x-4">
              <div className="w-8 h-8 bg-primary rounded-lg flex items-center justify-center">
                <span className="text-primary-foreground font-bold text-sm">SC</span>
              </div>
              <h1 className="text-2xl font-bold text-foreground">Saleem Care</h1>
            </div>
            <div className="text-sm text-muted-foreground">
              Medical Dashboard v2.1
            </div>
          </div>
        </div>
      </header>

      {/* Main Dashboard */}
      <main className="max-w-7xl mx-auto px-6 py-6 overflow-x-hidden">
        <PatientHeader />
        
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6 mt-6">
          {/* Left Column */}
          <div className="lg:col-span-2 space-y-6">
            <DiagnosticResults />
            <SymptomsAnalysis />
            <VitalSigns />
          </div>
          
          {/* Right Column */}
          <div className="space-y-6">
            <SeverityIndicator />
            <BodyModel />
          </div>
        </div>
        
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mt-6">
          <MedicalRecommendations />
          <PhysicianNotes />
        </div>
      </main>
    </div>
  );
};

export default AnalysisMedical;
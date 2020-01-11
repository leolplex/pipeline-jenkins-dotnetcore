pipeline {
    agent any    
    stages {
        stage('Build') {
            steps('Build Class library') {	
               sh "dotnet clean TDD/TDD.sln"
               sh "dotnet restore /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD/TDD.sln"
               sh "dotnet build /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD/TDD.sln"                             
            }
        }
         stage('UnitTests') {
            steps {                
              	sh returnStatus: true, script: "dotnet test TDD/TDD.sln --logger \"trx;LogFileName=/var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/unit_tests.xml\" --no-build /p:CollectCoverage=true /p:CoverletOutputFormat=opencover"
		        step([$class: 'MSTestPublisher', testResultsFile:"**/unit_tests.xml", failOnError: true, keepLongStdio: true])
            }
        }
        
        stage('Sonarqube') {
            steps {
                withSonarQubeEnv('sonarqube') {
                    // sh "dotnet test /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD/TDD.sln /p:CollectCoverage=true /p:CoverletOutputFormat=opencover"                    
                    sh "dotnet test /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD.API/TDD.API.csproj -l trx -r /results /p:CollectCoverage=true /p:CoverletOutput=/var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/results/coverage"
                    sh "dotnet test /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD/TDD.sln -l trx -r /results /p:CollectCoverage=true /p:CoverletOutput=/results/coverage /p:MergeWith='/var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/results/coverage.json'"
                    sh "dotnet test /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD/TDD.sln -l trx -r /results /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=/var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/results/coverage /p:MergeWith='/var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/results/coverage.json'"
                    sh "dotnet sonarscanner begin /k:\"NetCoreTDD\" /d:sonar.host.url=\"http://192.168.99.100:9000/\" /d:sonar.login=\"5eeaf1f8c47dc9ec40ee32b910fe3278e0215c6f\"  /d:sonar.cs.opencover.reportsPaths=\"/var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD.Test/coverage.opencover.xml\""
                    sh "dotnet  build  /var/jenkins_home/workspace/peline-jenkins-dotnetcore_master/TDD/TDD.sln"
                    sh "dotnet sonarscanner end /d:sonar.login=admin /d:sonar.password=admin"
                }
            }
        }
        stage("Quality Gate") {
            steps {
                timeout(time: 5, unit: 'MINUTES') {
                    // Parameter indicates whether to set pipeline to UNSTABLE if Quality Gate fails
                    // true = set pipeline to UNSTABLE, false = don't
                    waitForQualityGate abortPipeline: true
                }
            }
        }
    }
}



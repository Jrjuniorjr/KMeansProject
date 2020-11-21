library(factoextra)
library(FactoMineR)
library(cluster)
library(data.table)
library(tibble)
library(dplyr)

transactions <- read.csv("Transactions.csv", sep = ";")
transactions$X <- NULL
transactions$X.1 <- NULL
transactions$X.2 <- NULL
colnames(transactions)[1] <- "Customer"
colnames(transactions)[2] <- "Offer"

offerInformation <- read.csv("OfferInformation.csv", sep = ";")
colnames(offerInformation)[1] <- "Offer"

dados_brutos <- read.csv("DataPrepared.csv")
dados <- dados_brutos[, -33]
row.names(dados) <- dados_brutos[, 33]

set.seed(1)

for(i in 3:10){
  dados_kmeans <- kmeans(dados, i)
  Grupos <- as.data.frame(dados_kmeans$cluster) %>% rownames_to_column("Customer")
  colnames(Grupos)[2] <- "Grupo"
  j <- 1L
  while(j <= i){
    grupoByCustomers <- subset(Grupos, Grupos$Grupo == j)
    result <- merge(grupoByCustomers, transactions, by = "Customer")
    result <- merge(result, offerInformation, by = "Offer")
    result <- as.data.table(result)
    result <- result[, .N, by=Offer]
    colnames(result)[2] <- paste("Grupo", j, "com", i, "clusters")
    if(i == 3 && j == 1){
      resultFinal <- merge(offerInformation, result, by="Offer", all.x = TRUE)
    }
    else{
      resultFinal <- merge(resultFinal, result, by="Offer", all.x = TRUE) 
    }
    j <- j + 1L;
  }  
}
resultFinal[is.na(resultFinal)] <- 0
write.csv(resultFinal, "ResultsKMeans.csv")
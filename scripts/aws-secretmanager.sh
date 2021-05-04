#-------------------------------------------------------------------------------
# Get the value of a secret from Secrets Manager.
#
# Args:
#	$1	The secret id
#-------------------------------------------------------------------------------
getSecretValue() {
	local secretId=${1?param missing - secret-id}
	local secretValue=$(aws secretsmanager get-secret-value --secret-id "${secretId}")

	if [[ "${secretValue}" != *"error"* ]]; then
		echo "$secretValue" | jq -r '.SecretString' | tr -d '\n'
	fi
}

#-------------------------------------------------------------------------------
# Get the property from the value of a Secrets Manager secret.
#
# Args:
#	$1	The secret value
#	$2	The property to get a value from
#-------------------------------------------------------------------------------
getSecretValueProperty() {
	local secretValue=${1?param missing - secret-value}
	local secretProperty=${2?param missing - secret-property}

	echo "$secretValue" | jq -r ".${secretProperty}" | tr -d '\n'
}